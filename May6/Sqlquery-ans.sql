use pubs

select * from titles;

Select title As title_names from titles;

Select * from titles where pub_id = 1389;

Select title from titles where price between 10 and 15;

Select * from titles where price is null;

select title as title_starts_with_THE from titles where title like 'The%';

select title from titles where title not like '%v%';

select title,royalty from titles order by royalty;

select title,pub_id,type,price from titles order by pub_id DESC, type ASC, price DESC;

select type,AVG(price) AS average_price from titles Group by type;

select distinct type from titles;

select TOP 2 price from titles order by price DESC;

select * from titles where type='business' and price < 20 and advance > 7000;

Select pub_id, COUNT(*) AS book_count FROM titles WHERE price BETWEEN 15 AND 25 AND Lower(title) like '%it%' GROUP BY pub_id HAVING COUNT(*) > 2 ORDER BY book_count;

select * from authors;

select * from authors where state='CA';

select state,COUNT(*) from authors group by state;
