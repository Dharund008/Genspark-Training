--select queries

--1)List all films with their length and rental rate, sorted by length descending

select * from film
select title, length, rental_rate from film order by length DESC

--2)Find the top 5 customers who have rented the most films.
--Hint: Use the rental and customer tables.

select * from rental;
select * from rental where customer_id = 484;
select * from customer;

select c.customer_id, c.first_name || ' ' || c.last_name as Customer_name,
count(r.rental_id) as Total_rents
from customer c
join rental r on c.customer_id = r.customer_id
group by c.customer_id, c.first_name,c.last_name
order by Total_rents DESC
LIMIT 5; --use || in concat instead of +


--3)Display all films that have never been rented.
--Hint: Use LEFT JOIN between film and inventory → rental.
select * from film;
select * from inventory;
select * from rental;

select distinct f.film_id,f.title, count(r.rental_id) as total_rents from film f 
left outer join inventory i
on f.film_id = i.film_id left outer join rental r on
i.inventory_id = r.inventory_id 
group by f.film_id
having count(r.rental_id) = 0
order by f.film_id;

-- select f.film_id, count(r.rental_id) as rentedcount
-- from film f left outer join inventory i on f.film_id = i.film_id
-- left outer join rental r on i.inventory_id=r.inventory_id
-- group by f.film_id
-- having count(r.rental_id)=0
-- order by f.film_id

select rental_id,inventory_id from rental where inventory_id in (1,9)


--join queries

--4)List all actors who appeared in the film ‘Academy Dinosaur’.
--Tables: film, film_actor, actor

select * from film
select * from film_actor
select * from actor

-- select first_name, last_name from actor where actor_id in(
-- select actor_id from film_actor where film_id =(
-- select film_id from film where title='Academy Dinosaur'))

select a.first_name || ' ' || a.last_name as Actor_name, a.actor_id, f.title
from actor a
join film_actor fa on a.actor_id = fa.actor_id join film f on
f.film_id = fa.film_id where f.title = 'Academy Dinosaur'


--5) List each customer along with the total number of rentals they made and the total amount paid.
--Tables: customer, rental, payment

select * from rental
select * from customer
select * from payment
select customer_id,count(rental_id) from payment where customer_id = 1
group by customer_id order by customer_id;


select c.customer_id, c.first_name || ' ' || c.last_name as Customer_name,
count(r.rental_id) as Total_rents, SUM(p.amount) as Total_amount from customer c
join rental r on c.customer_id = r.rental_id join payment p
on p.customer_id = c.customer_id
group by c.customer_id, c.first_name,c.last_name
order by c.customer_id

select c.customer_id, concat(c.first_name,' ',c.last_name) as Customer_name,
count(r.rental_id) as Total_rents, SUM(p.amount) as Total_amount from customer c
join rental r on c.customer_id = r.rental_id join payment p
on p.customer_id = c.customer_id
group by c.customer_id, c.first_name,c.last_name
order by c.customer_id


--CTE-Based Queries

--6) Using a CTE, show the top 3 rented movies by number of rentals.
--Columns: title, rental_count
select * from film
select * from rental

with cte_movies
as
(
	select f.title as title, count(r.rental_id) as rental_count from film f 
	left outer join inventory i
	on f.film_id = i.film_id left outer join rental r on
	i.inventory_id = r.inventory_id 
	group by f.film_id
	order by f.film_id DESC	
)

select * from cte_movies LIMIT 3;

--7)Find customers who have rented more than the average number of films.
--Use a CTE to compute the average rentals per customer, then filter.

select * from customer
select * from rental

with cte_rent
as
(
	select customer_id, Count(rental_id) as rental_count from rental 
	group by customer_id;
),
cte_avg 
as
(
	select avg(rental_count) as avg_rentals from cte_rent
)

SELECT 
    c.customer_id, 
    c.first_name || ' ' || c.last_name AS customer_name,
    crc.rental_count
FROM cte_rent crc
JOIN customer c ON crc.customer_id = c.customer_id
JOIN cte_avg ar ON 
WHERE crc.rental_count > ar.avg_rentals
ORDER BY crc.rental_count DESC;



--Function Questions

--8)Write a function that returns the total number of rentals for a given customer ID.
--Function: get_total_rentals(customer_id INT)

create or replace function get_total_rentals(fcust_id int)
returns INT as
$$
declare Total_rentals int;
begin
	select count(*) into Total_rentals from rental where customer_id = fcust_id;
	return Total_rentals;
end;
$$
LANGUAGE plpgsql;

select get_total_rentals(1);


--Stored Procedure Questions

--9)Write a stored procedure that updates the rental rate of a film by film ID and new rate.
--Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)


create procedure proc_UpdateRentals(pfilm_id int, new_rate numeric)
LANGUAGE plpgsql
as $$
begin
	update film set rental_rate = new_rate where film_id = pfilm_id;
end;
$$;

call proc_UpdateRentals(8,3.09)

select * from film where film_id=8;


--10)Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
--Procedure: get_overdue_rentals() that selects relevant columns.
select * from rental


create or replace procedure proc_OverdueRentals()
LANGUAGE plpgsql
as $$
declare rec record;
begin
	for rec in 
		select * from rental where return_date is null and
		rental_date < NOW() - INTERVAL '7 days'
	loop
		raise notice 'Rental ID: %, Customer ID: %, Date: %', 
            rec.rental_id, rec.customer_id, rec.rental_date;
	end loop;
end;
$$;

call proc_OverdueRentals()












