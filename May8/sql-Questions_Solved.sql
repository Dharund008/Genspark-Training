use pubs


--used nortwind data 
select * from orders;
select * from [Order Details];
select * from employees;

select * from products;
select * from suppliers;
select * from categories;

select * from customers;
select * from products;

--May8 task questions:

--1)List all orders with the customer name and the employee who handled the order.
select OrderID,ContactName,concat(FirstName ,' ',  LastName) as EmployeeName from orders join
customers on orders.customerID = customers.customerID join employees on
orders.employeeID = employees.employeeID;

--2) Get a list of products along with their category and supplier name.
select ProductID, ProductName, categories.categoryID, ContactName from products join 
categories on products.CategoryID = categories.CategoryID join 
suppliers on products.SupplierID = suppliers.SupplierID
order by 3;


--3) Show all orders and the products included in each order with quantity and unit price.
select o.OrderID, ProductName, od.Quantity, od.UnitPrice from orders o join [Order Details] od on
o.OrderID = od.OrderID join Products on
od.ProductID = products.ProductID;


--4) List employees who report to other employees (manager-subordinate relationship).
select * from employees;
select e.EmployeeID as SubordinateID, concat(e.FirstName ,' ',  e.LastName) as SubordinateName,m.EmployeeID as ManagerID,
concat(m.FirstName ,' ',  m.LastName) as ManagerName
from employees e join employees m on e.ReportsTo = m.EmployeeID;


--5) Display each customer and their total order count.
select c.CustomerID, c.ContactName as CustomerName, Count(o.OrderID) as Total_orders from customers c join orders o
on c.CustomerID = o.CustomerID Group by c.CustomerID, c.ContactName order by Total_orders DESC;

--6) Find the average unit price of products per category.
select AVG(UnitPrice) as Average_UnitPrice from products group by CategoryID order by Average_UnitPrice DESC;


--7)List customers where the contact title starts with 'Owner'.
select CustomerID, ContactName from customers where ContactTitle like 'Owner%';

--8) Show the top 5 most expensive products.
select TOP 5 ProductID, ProductName, UnitPrice from Products order by UnitPrice DESC;

--9)Return the total sales amount (quantity × unit price) per order.
select od.OrderID, SUM(od.Quantity * od.UnitPrice) as Total_Sales from [Order Details] od
group by od.OrderID;

--10)Create a stored procedure that returns all orders for a given customer ID.

create or alter procedure 
proc_GetAllOrders(@pcustomerID nvarchar(20))
as
begin
	select * from Orders where CustomerID = @pcustomerID;
end

exec proc_GetAllOrders 'VINET'

--11)Write a stored procedure that inserts a new product.
create or alter procedure
proc_GetInsert(@pname nvarchar(max), @psuppid int, @pcatid int, @punitp float)
as
begin
	insert into products (productname,supplierid, categoryid, quantityperunit,
	unitprice, unitsinstock, unitsonorder, reorderlevel, discontinued) values
	(@pname,@psuppid,@pcatid,'12 per dozen', @punitp, 29, 0,0,0)
end

exec proc_GetInsert 'Egg',8,8,5.40
select * from products where ProductName = 'Egg';




--12) Create a stored procedure that returns total sales per employee.
create or alter procedure
proc_GetTotalSales(@pempID int)
as
begin
	select o.EmployeeID, SUM(od.Quantity * od.UnitPrice) as Total_Sales from orders o join
	[Order Details] od on o.OrderID = od.OrderID  where o.EmployeeID = @pempID group by o.EmployeeID;
end

exec proc_GetTotalSales 3


--13)Use a CTE to rank products by unit price within each category.

with cteProductsRanking 
as
(
	select productID, productName, CategoryID, UnitPrice, ROW_NUMBER() over (partition by categoryID order by UnitPrice DESC) as PriceRank
	from products
)

select * from cteProductsRanking;

--14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.
create or alter procedure
proc_GetRevenue(@pid int)
as
begin 
	with cte_revenue as
	(
	select ProductId, sum(unitprice*quantity) as Amount from [order details] group by productid
	)

	select * from cte_revenue where productId = @pid;
end

exec proc_GetRevenue 11


--15) Use a CTE with recursion to display employee hierarchy.
WITH EmployeeHierarchy AS (
    -- Anchor :Top-level employees (no manager)
    SELECT
        EmployeeID,
        FirstName + ' ' + LastName AS EmployeeName,
        ReportsTo,
        1 AS Level
    FROM Employees
    WHERE ReportsTo IS NULL

    UNION ALL

    -- Recursive member: Employees reporting to the above
    SELECT
        e.EmployeeID,
        e.FirstName + ' ' + e.LastName AS EmployeeName,
        e.ReportsTo,
        eh.Level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh
        ON e.ReportsTo = eh.EmployeeID
)
SELECT *
FROM EmployeeHierarchy
ORDER BY Level, ReportsTo, EmployeeID;