--transactions,triggers,cursors

--cursor based questions

--1)Write a cursor that loops through all films and prints titles longer than 120 minutes.
--cursor syntax : declare cursor_name cursor


--explicit
do $$
declare rec record; cur cursor for select * from film; --declaring record and cursor
begin
	open cur; --open cursor
	loop
		fetch cur into rec;
		exit when not found;
		if rec.length > 120 then
			raise notice 'ID: %,Title : %,Duration : %',
			rec.film_id,rec.title,rec.length;
		end if;
	end loop;
end 
$$


--implicit
do $$
declare rec record;
begin
	for rec in select * from film where length > 120
	loop
		raise notice 'ID: %,Title : %,Duration : %',
		rec.film_id,rec.title,rec.length;
	end loop;
end;
$$;



--2) Create a cursor that iterates through all customers and counts how many rentals each made.

--implicit
-- do $$
-- declare rec record; total_rentals int;
-- begin
-- 	for rec in select * from customer
-- 	loop
-- 		select count(*) into total_rentals from rental where customer_id = rec.customer_id order by total_rentals DESC;
-- 		raise notice 'ID : %,FirstName : %,LastName : %,rental_count : %',rec.customer_id,
-- 		rec.first_name,rec.last_name,total_rentals;
-- 	end loop;
-- end $$;

--explicit
do $$
declare rec record;first_name text; cur cursor for (select customer_id, count(*) as total_rentals from rental
group by customer_id);
begin
	open cur;
	Loop
		fetch cur into rec;
		exit when not found;
		select c.first_name into first_name from customer c where c.customer_id = rec.customer_id;
		raise notice 'ID : %,first_name: %,rental_count : %',rec.customer_id,
			first_name,rec.total_rentals;
	end loop;
	close cur;
end $$;

 


--3)Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.
select * from film where film_id=14;

--explicit
do $$
declare rec record;cur cursor for (select f.film_id,count(r.rental_id) as rental_count
from film f left join inventory i on f.film_id = i.film_id left join rental r on
i.inventory_id = r.inventory_id group by f.film_id);
begin
	open cur;
	loop
		fetch cur into rec;
		exit when not found;
		if rec.rental_count < 5 then
			update film set rental_rate = rental_rate + 1 where film_id = rec.film_id;
			raise notice 'Updated film_id %: rental count = %, rate increased by $1',
	                rec.film_id, rec.rental_count;
		end if;
	end loop;
	close cur;
end $$;


--4)Create a function using a cursor that collects titles of all films from a particular category.

select * from category
select * from film

create or replace function func_category(fcat varchar(30))
returns table(film_name varchar(30))
as $$
declare rec record;
begin
	for rec in select film.title as Title from film_category f join category c
			on f.category_id=c.category_id join film on film.film_id = f.film_id
			where c.name = fcat
	loop
		film_name:= rec.Title;
		return next;
	end loop;
end $$
language plpgsql;
 
select * from func_category('Documentary')



--5) Loop through all stores and count how many distinct films are available in each store using a cursor.

 select * from store;
select * from inventory;
 select * from film;

 do $$
 declare rec record;film_Count int;cur cursor for (select store_id from store);
 begin
 	open cur;
	loop
		fetch cur into rec;
		exit when not found;
		select count(distinct film_id) into film_Count from inventory where
		store_id = rec.store_id;
 
 		raise notice 'store ID : %,Distinct_Films : %',rec.store_id,film_Count;
	end loop;
	close cur;
end $$;



--trigger based questions
































--transaction based questions

--1)Write a transaction that inserts a customer and an initial rental in one atomic operation.

-- select * from customer;

-- BEGIN;

-- -- Step 1: Insert customer and get ID using CTE
-- WITH new_cust AS (
--   INSERT INTO customer (
--     store_id, first_name, last_name, email, address_id, activebool,
--     create_date, last_update, active
--   )
--   VALUES (
--     1, 'Eren', 'Yeager', 'erenyeager@transaction.org', 605, true,
--     '2025-09-05', '2013-05-26 14:49:45.738', 1
--   )
--   RETURNING customer_id
-- )

-- -- Step 2: Insert rental using the new customer_id
-- INSERT INTO rental (
--   rental_date, inventory_id, customer_id, staff_id, last_update
-- )
-- SELECT NOW(), 1, customer_id, 1, NOW()
-- FROM new_cust;




