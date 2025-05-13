/*May13 -- Triggers

Trigger :define set of querie/work that has to be executed when an action is done.

in postgresql, triggers can used within functions, but not in others
one trigger with function
one func with more triggers
*/
create table audit_log
(
audit_id serial primary key ,
table_name text,
fieldname text,
old_value text,
new_value text,
updated_date timestamp default current_Timestamp
);

select * from customer;

create or replace function fn_Update_Audit_log() --creating function ; every func returns something unlike sp
returns trigger 		--here,returns trigger
as $$
begin
	insert into audit_log (table_name,fieldname,old_value,new_value,updated_date)
	values ('customer','email',OLD.email,NEW.email,current_Timestamp);
	return new;
end;
$$
LANGUAGE plpgsql;


create trigger trg_log_customer_email_change --creating trigger
before update 
on customer
for each row
execute function fn_Update_Audit_log();

drop trigger trg_log_customer_email_change  on customer -- droping trigger on which table

drop table audit_log;

update customer set email= 'cust1.smiths@sakilcustomer.org' where customer_id = 1;
-- View the updated customer table
SELECT * FROM customer ORDER BY customer_id;

-- View the audit log
SELECT * FROM audit_log;



--2nd method :json - focus entire row
create or replace function Update_Audit_log()
returns trigger 
as $$
declare 
   col_name text := TG_ARGV[0];
   tab_name text := TG_ARGV[1];
   o_value text;
   n_value text;
begin
    o_value := row_to_json(old);
	n_value := row_to_json(new);
	if o_value is distinct from n_value then
		Insert into audit_log(table_name,fieldname,old_value,new_value,updated_date) 
		values(tab_name,col_name,o_value,n_value,current_Timestamp);
	end if;
	return new;
end;
$$ language plpgsql

create trigger trg_log_customer_email_change --creating trigger
before update 
on customer
for each row
execute function Update_Audit_log('email','customer');


drop trigger trg_log_customer_email_change on customer;

update customer set email= 'patricia.johnsom@sakilcustomer.org' where customer_id = 2;

SELECT * FROM customer ORDER BY customer_id;

SELECT * FROM audit_log;


--3rd method: dynamically extract - focus on just one field
create or replace function Update_Audit_log()
returns trigger 
as $$
declare 
   col_name text := TG_ARGV[0];
   tab_name text := TG_ARGV[1];
   o_value text;
   n_value text;
begin
    EXECUTE FORMAT('select ($1).%I::TEXT', COL_NAME) INTO O_VALUE USING OLD;
    EXECUTE FORMAT('select ($1).%I::TEXT', COL_NAME) INTO N_VALUE USING NEW;
	if o_value is distinct from n_value then
		Insert into audit_log(table_name,fieldname,old_value,new_value,updated_date) 
		values(tab_name,col_name,o_value,n_value,current_Timestamp);
	end if;
	return new;
end;
$$ language plpgsql

create trigger trg_log_customer_email_change --creating trigger
before update 
on customer
for each row
execute function Update_Audit_log('last_name','customer');


drop trigger trg_log_customer_email_change on customer;

update customer set last_name= 'SM' where customer_id = 1;

SELECT * FROM customer ORDER BY customer_id;

SELECT * FROM audit_log;




--cursors:

do $$
declare
    rental_record record;
    rental_cursor cursor for
        select r.rental_id, c.first_name, c.last_name, r.rental_date
        from rental r
        join customer c on r.customer_id = c.customer_id
        order by r.rental_id;
begin
    open rental_cursor;

    loop
        fetch rental_cursor into rental_record;
        exit when not found;

        raise notice 'rental id: %, customer: % %, date: %',
                     rental_record.rental_id,
                     rental_record.first_name,
                     rental_record.last_name,
                     rental_record.rental_date;
    end loop;

    close rental_cursor;
end;
$$;


--can be used to insert records into the another table too.
create table rental_tax_log (
    rental_id int,
    customer_name text,
    rental_date timestamp,
    amount numeric,
    tax numeric
);

select * from rental_tax_log order by rental_id;

do $$
declare
    rec record;
    cur cursor for
        select r.rental_id, 
               c.first_name || ' ' || c.last_name as customer_name,
               r.rental_date,
               p.amount
        from rental r
        join payment p on r.rental_id = p.rental_id
        join customer c on r.customer_id = c.customer_id
		order by rental_id;
begin
    open cur;

    loop
        fetch cur into rec;
        exit when not found;

        insert into rental_tax_log (rental_id, customer_name, rental_date, amount, tax)
        values (
            rec.rental_id,
            rec.customer_name,
            rec.rental_date,
            rec.amount,
            rec.amount * 0.10
        );
    end loop;

    close cur;
end;
$$;
-------------






