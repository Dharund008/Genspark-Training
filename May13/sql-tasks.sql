--May13 Tasks:

--Cursors:

--1)Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.
create table summary(
customer_id int,
customer_name text,
rental_count int
)
select * from customer;
select * from rental;

do $$
declare
	rec record;
	cur cursor for
		select c.customer_id,
               c.first_name || ' ' || c.last_name as customer_name,
			   count(*) as rental_count from rental r 
			   join customer c on r.customer_id = c.customer_id
			   group by c.customer_id,c.first_name,c.last_name
			   order by rental_count DESC;
begin
	open cur;

	loop
		fetch cur into rec;
		exit when not found;

		insert into summary(customer_id, customer_name, rental_count) values
		(
			rec.customer_id,
			rec.customer_name,
			rec.rental_count
		);
	end loop;

	close cur;
end;
$$

select * from summary;

--2)Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.
select * from film_Category;
select * from category;
select * from film;
select * from rental order by inventory_id;
select * from inventory;

do $$
declare
	rec record;
	cur cursor for select f.film_id,f.title,count(*) as total_rented from film f join 
	inventory i on f.film_id = i.film_id join rental r on i.inventory_id = r.inventory_id
	group by f.film_id,f.title 
	order by total_rented DESC;
begin

	open cur;

	loop
		fetch cur into rec;
		exit when not found;

		if rec.total_rented > 10 then
			if exists (
				select 1 from film_category fc join category c on fc.category_id = c.category_id
				join film f on f.film_id = rec.film_id and c.name = 'Comedy'
			) then
			raise notice '% film under Comedy category rented more than 10 times!',rec.title;
			end if;
		end if;
	end loop;

	close cur;
end;
$$

/*
3)Create a cursor to go through each store 
and count the number of distinct films available, and insert results into a report table.
*/
create table report
(
store_id int,
distinct_films int 
)
select * from film order by film_id DESC;
select * from inventory;
select * from store;
drop table report;
do $$
declare 
	rec record;
	distinct_films int;
	cur cursor for select store_id from store;
begin
	open cur;

	loop
		fetch cur into rec;
		exit when not found;

		select count(distinct film_id) into distinct_films from inventory where store_id = rec.store_id;
		Insert into report(store_id,distinct_films) values (rec.store_id,distinct_films);

	end loop;

	close cur;
end;
$$;
truncate table report;
select * from report;



/*
--4)Loop through all customers who haven't rented in the last 6 months
and insert their details into an inactive_customers table.

*/
select * from customer;
select * from rental order by customer_id DESC;

create table inactive_customer
(
customer_id int,
customer_name text,
last_rented_date text
)

do $$
declare
	rec record;
	cur cursor for select c.customer_id,
               c.first_name || ' ' || c.last_name as customer_name,
			   r.rental_date
				from customer c join rental r on c.customer_id = r.customer_id
				where r.rental_date < current_date - interval '6 months'
				order by customer_id;

begin
	open cur;

	loop
		fetch cur into rec;
		exit when not found;

		insert into inactive_customer(customer_id,customer_name,last_rented_date) values 
		(rec.customer_id,rec.customer_name,rec.rental_date);
	end loop;

	close cur;
end;
$$

select * from inactive_customer;

---Transactions


--1)Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.
do $$
declare
	new_customerid int;
	new_rentalid int;
begin
	insert into customer(store_id, first_name, last_name, email, address_id, active, create_date) values
	(1,'sam','anderson','sam.anderson@sakilacusotmer.org',5,1,now())
	returning customer_id into new_customerid;

	insert into rental (rental_date, inventory_id, customer_id, staff_id) values
	(now(),101,new_customerid,1) returning rental_id into new_rentalid;

	insert into payment (customer_id, staff_id, rental_id, amount, payment_date)
	values (new_customerid,1,new_rentalid,2.00,now());

	commit;
end;
$$;

select * from customer where first_name = 'sam'; --cust id - 601
select * from rental where customer_id = 601;
select * from payment where date(payment_date) = current_Date; --checking 

/*
2)Simulate a transaction where one update fails (e.g., invalid rental ID), 
and ensure the entire transaction rolls back.
*/
select * from rental;

do $$
begin
	update rental set rental_date = current_date + Interval '7 days' where customer_id = 601; 


	if exists (select 1 from rental where customer_id = 603) then
		update rental set staff_id = 2 where customer_id = 603;
	else
		raise notice 'Update fails';
		raise exception 'No data found - forcing rollback';
		return;
	end if;

	commit;
	exception when others then
		raise notice 'Transaction rolled back - error!';
end;
$$;

/*
3)Use SAVEPOINT to update multiple payment amounts. 
Roll back only one payment update using ROLLBACK TO SAVEPOINT.
*/
select * from payment;

begin;	
	update payment set amount = amount - 1 where customer_id = 601;
	savepoint pointA;

	update payment set amount = amount + 1 where customer_id = 602;
	savepoint pointB;

	update payment set amount = amount + 2.3 where customer_id = 604;
	savepoint pointC;

	rollback to pointb;

	update payment set amount = amount + 2.3 where customer_id = 603;
	savepoint pointC;

	commit;
end;

select * from payment where date(payment_date) = current_Date;

--4)Perform a transaction that transfers inventory from one store to another (delete + insert) safely.




	
/*
5)Create a transaction that deletes a customer and all associated records (rental, payment), 
ensuring referential integrity.
*/







--Triggers
--1)Create a trigger to prevent inserting payments of zero or negative amount.

select * from payment order by rental_id DESC;

create or replace function fn_payment()
returns trigger
as $$
begin
	if new.amount <= 0 then
		raise exception 'Payment must be greater than 0! Given :%',new.amount;
	end if;
	return new;
end;
$$
language plpgsql;

create trigger trg_prevent_invalid_payment
before insert on
payment
for each row
execute function fn_payment();

insert into payment (customer_id, staff_id, rental_id, amount, payment_date)
values (393,2,16050,-1,current_timestamp);

/*
2)Set up a trigger that automatically updates last_update on the film table
when the title or rental rate is changed.
*/
select * from film;

create or replace function fn_automatic_update()
returns trigger
as $$
begin	
	if new.title is distinct from old.title or
		new.rental_rate is distinct from old.rental_rate then
		new.last_update = current_timestamp;
	end if;
	return new;
end;
$$
language plpgsql;

create trigger tr_update
after update on film
for each row
execute function fn_automatic_update();


update film set rental_rate = rental_rate + 1 where film_id = 1;

select * from film order by film_id;

/*
--3) Write a trigger that inserts a log into rental_log
whenever a film is rented more than 3 times in a week.
*/
create table rental_log (
    rental_id int,
    inventory_id int,
    last_update timestamp default current_Timestamp
);

select * from rental;

create or replace function fn_insert_log()
returns trigger
as $$
declare 
	filmid int;
	rental_count int;
begin
	
	select i.film_id into filmid from inventory i join film f on i.inventory_id = new.inventory_id;

	select count(*) into rental_count from rental r join inventory i on r.inventory_id = i.inventory_id 
		where i.film_id = filmid and r.rental_date >= current_Date - interval '7 days';

	if rental_count > 3 then
			insert into rental_log (rental_id, inventory_id) values(new.rental_id,new.inventory_id);
	end if;
	return new;
end;
$$
language plpgsql;


create trigger tr_log_insert
after insert on rental
for each row 
execute function fn_insert_log();


select * from rental order by rental_id DESC;
select * from rental_log;

INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (16050, CURRENT_DATE, 50, 1, NULL, 1, CURRENT_TIMESTAMP);

INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (16051, CURRENT_DATE, 50, 1, NULL, 1, CURRENT_TIMESTAMP);


INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (16052, CURRENT_DATE, 50, 1, NULL, 1, CURRENT_TIMESTAMP);


INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (16053, CURRENT_DATE, 50, 1, NULL, 1, CURRENT_TIMESTAMP);



