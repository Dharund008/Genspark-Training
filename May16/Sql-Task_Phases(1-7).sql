/*Phase 1:You are tasked with building a PostgreSQL-backed database for an EdTech company
that manages online training and certification programs for individuals across various technologies.

The goal is to:

Design a normalized schema

Support querying of training data

Ensure secure access

Maintain data integrity and control over transactional updates

Database planning (Nomalized till 3NF)

A student can enroll in multiple courses

Each course is led by one trainer

Students can receive a certificate after passing

Each certificate has a unique serial number

Trainers may teach multiple courses
*/

enrolloing-status
id,message -{completed,ongoing,dropped}

purchase-status
id,message -{paid,error}

grade-table
id,grade,description -> {1,A+,more than 95%}

course-category
id,category(domain) ->{1,cloud}

courses
id,course_name,description,category_id,trainer_id,duration,amount

student
id,name,email,ph_number,dob


trainer
id,name,email,expertise

student-course
id,student_id,course_id,date,enroll-statusid

course_purchase
id,stud_course id,amount,date,status


certificate
id,student_course id,issue date, gradeid

--another enhacement is :- we can have an separate trainer_course table..

---------

/*
Phase 2: DDL & DML

* Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)
* Insert sample data using `INSERT` statements
* Create indexes on `student_id`, `email`, and `course_id`
*/
--1)--with the plan given,
1. **students**
   * `student_id (PK)`, `name`, `email`, `phone`
2. **courses**
   * `course_id (PK)`, `course_name`, `category`, `duration_days`
3. **trainers**
   * `trainer_id (PK)`, `trainer_name`, `expertise`
4. **enrollmentsnrollment**
   * `enrollment_id (PK)`, `student_id (FK)`, `course_id (FK)`, `enroll_date`
5. **certificates**
   * `certificate_id (PK)`, `enrollment_id (FK)`, `issue_date`, `serial_no`
6. **course\_trainers** (Many-to-Many if needed)
   * `course_id`, `trainer_id`

create table students (
    student_id serial primary key,
    name varchar(100) not null,
    email varchar(100) unique not null,
    phone varchar(15) unique not null
);

create table courses (
    course_id serial primary key,
    course_name varchar(100) not null,
    category varchar(50),
    duration_days int check (duration_days > 0)
);

create table trainers (
    trainer_id serial primary key,
    trainer_name varchar(100) not null,
    expertise varchar(100) not null
);

create table enrollments (
    enrollment_id serial primary key,
    student_id integer not null,
    course_id integer not null,
	enroll_date date default current_date,
	constraint fk_en_sid foreign key (student_id) references students(student_id),
	constraint fk_en_cid foreign key (course_id) references courses(course_id),
	unique(student_id, course_id)
);

create table certificates (
    certificate_id serial primary key,
    enrollment_id integer,
    issue_date date not null default current_date,
    serial_no varchar(50) unique not null,
	constraint fk_cer_eid foreign key (enrollment_id) references enrollments(enrollment_id)
);

create table course_trainers (
    course_id integer,
    trainer_id integer,
    primary key (course_id, trainer_id),
	constraint fk_ct_cid FOREIGN KEY (course_id) REFERENCES courses(course_id),
    constraint fk_ct_tid FOREIGN KEY (trainer_id) REFERENCES trainers(trainer_id)

);

--2)
insert into students (name,email,phone) values
('Alice Smith', 'alice@example.com', '1111111111'),
('Bob Johnson', 'bob@example.com', '2222222222'),
('Charlie Lee', 'charlie@example.com', '3333333333'),
('Diana Patel', 'diana@example.com', '4444444444'),
('Ethan Brown', 'ethan@example.com', '5555555555'),
('Fiona Davis', 'fiona@example.com', '6666666666'),
('George Miller', 'george@example.com', '7777777777');

select * from students;


insert into courses (course_name, category, duration_days) values
('Data Science 101', 'Data Science', 30),
('Web Development', 'Programming', 45),
('Cybersecurity Basics', 'Security', 20),
('Machine Learning', 'AI', 60),
('Cloud Fundamentals', 'Cloud Computing', 25),
('Database Design', 'Backend', 35),
('AI Fundamentals', 'AI/ML', 50);

insert into trainers (trainer_name, expertise) VALUES
('John Doe', 'Data Science'),
('Jane Roe', 'Web Development'),
('Mark Lee', 'Cybersecurity'),
('Nina Patel', 'AI/Machine Learning'),
('Chris Evans', 'Cloud Computing');

insert into enrollments (student_id, course_id) values
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(1, 3),
(2, 4),
(3, 7);


iNSERT INTO certificates (enrollment_id, serial_no) VALUES
(1, 'CERT2025-001'),
(2, 'CERT2025-002'),
(3, 'CERT2025-003'),
(4, 'CERT2025-004'),
(5, 'CERT2025-005'),
(6, 'CERT2025-006');

select * from courses;

insert into course_trainers (course_id, trainer_id) values
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 2),
(7, 4);

--3)
create index idx_student_sid on students(student_id);

create index idx_student_em on students(email);

create index idx_course_cid on courses(course_id);




/*
Phase 3: SQL Joins Practice

Write queries to:

1. List students and the courses they enrolled in
2. Show students who received certificates with trainer names
3. Count number of students per course

---
*/
--1)
select s.student_id,s.name AS student_name,c.course_id,c.course_name from students s
join enrollments e on s.student_id = e.student_id join
courses c on c.course_id = e.course_id
order by s.student_id;

--2)
select e.student_id,e.course_id,cr.serial_no,t.trainer_name from enrollments e join
certificates cr on e.enrollment_id = cr.enrollment_id join course_trainers ct on
e.course_id = ct.course_id join trainers t on ct.trainer_id = t.trainer_id;

--3)
select c.course_id,c.course_name,count(e.student_id) as Total_students from courses c join
enrollments e on c.course_id = e.course_id group by c.course_id,c.course_name 
order by c.course_id;


/*
Phase 4: Functions & Stored Procedures

Function:

Create `get_certified_students(course_id INT)`
→ Returns a list of students who completed the given course and received certificates.

Stored Procedure:

Create `sp_enroll_student(p_student_id, p_course_id)`
→ Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).
*/

--function:
create or replace function get_certified_students(f_course_id int)
returns table(
	student_id int,
	student_name varchar,
	email varchar
)
as $$
begin
	return query
	select s.student_id,s.name,s.email from students s join enrollments e
	on s.student_id = e.student_id join certificates cr on
	e.enrollment_id = cr.enrollment_id  where e.course_id = f_course_id;
end;
$$ language plpgsql;

select get_certified_students(1);

iNSERT INTO certificates (enrollment_id, serial_no) VALUES
(7, 'CERT2025-007');

select get_certified_students(3);


--store_procedure:
create or replace procedure sp_enroll_student(p_student_id int, p_course_id int,status boolean)
as $$
declare
	enrol_id int;
begin
	insert into enrollments (student_id, course_id) values (p_student_id, p_course_id) 
	returning enrollment_id into enrol_id;

	if status = true then
		insert into certificates (enrollment_id, serial_no) values (enrol_id, 'CERT2025-008');
	else
		raise notice 'Conditional Insert failed!';
	end if;
end;
$$ language plpgsql;

call sp_enroll_student(4,5,true);


select * from certificates;

call sp_enroll_student(5,6,false);

/*
Phase 5: Cursor

Use a cursor to:

* Loop through all students in a course
* Print name and email of those who do not yet have certificates

---
*/

--5)
do $$
declare 
	rec record;
	cur cursor for (select * from students s
join enrollments e on s.student_id = e.student_id left join
certificates c on c.enrollment_id = e.enrollment_id
order by s.student_id);
begin
	open cur;

	loop
		fetch cur into rec;
		exit when not found;

		if (rec.certificate_id is null) then
			raise notice 'Student name :%, Student email :%',rec.name,rec.email;
		end if;

	end loop;

	close cur;
end;
$$ language plpgsql;


/*Phase 6: Security & Roles

1. Create a `readonly_user` role:

   * Can run `SELECT` on `students`, `courses`, and `certificates`
   * Cannot `INSERT`, `UPDATE`, or `DELETE`
   

2. Create a `data_entry_user` role:

   * Can `INSERT` into `students`, `enrollments`
   * Cannot modify certificates directly

---
*/
--1)
create role readonly_user login password 'readonly_pass';

grant select on students, courses, certificates to readonly_user;

select * from students;

revoke insert, update, delete on students, courses, certificates from readonly_user;

update students set phone = '1234567890' where student_id = 1;
/*it updates because, the session here is main user..so the go another user,

set role 'role_name' ->set role readonly_user

to check which user is been logged on currently,
select current_user(user/role);

select rolname from pg_roles; -- to check every roles(roles available)

select rolname from pg_roles where rolcanlogin; --to check the user we have created

after this command, check again with update command, returns:- permission denied.

you can also go back to main user

reset role;
*/

--2)
create role data_entry_user login password 'dataentry_pass';

grant insert on students,enrollments to data_entry_user;

revoke insert,update,delete on certificates from data_entry_user;

/*
here, select * from students; -- wont run ->returns permission denied
because it isnt been granted here in this role.
only the granted privileges will be executed.

also the revoked privileges also wont be executed
update certificates set serial_no = 'CERT2025 - 000' where certificate_id = 1;
--returns, permission denied.
*/
set role readonly_user;

select * from students;

update students set phone = '2222211111' where student_id = 2; --permission dendied

reset role;

set role data_entry_user;
select * from students;

update certificates set serial_no = 'CERT2025 - 000' where certificate_id = 1;

select current_user;

select rolname from pg_roles;

select rolname from pg_roles where rolcanlogin;

/*
Phase 7: Transactions & Atomicity

Write a transaction block that:

* Enrolls a student
* Issues a certificate
* Fails if certificate generation fails (rollback)

```sql
BEGIN;
-- insert into enrollments
-- insert into certificates
-- COMMIT or ROLLBACK on error
```
*/

create or replace procedure proc_transac(t_sid int,t_cid int,status boolean)
language plpgsql
as $$
declare
	enrol_id int;
begin
	begin
		insert into enrollments (student_id, course_id) values (t_sid, t_cid) 
		returning enrollment_id into enrol_id;
	
		if status = true then
			insert into certificates (enrollment_id, serial_no) values (enrol_id, 'CERT2025-009');
		end if;
	
	exception when others then
			raise notice 'Error raised : %',sqlerrm;
	end;
end;
$$; 

call proc_transac(4,2,false);

select * from enrollments;



		