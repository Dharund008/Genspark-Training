--Transaction 
--group of sql statments treated as single unit to maintain data integrity and consistency.
/*
Acid Properties on Transactions
1)Atomicity - either transaction succeeds or fails
2)Consistency - data must remain consistent before and after transaction : no invalid state
one valid state to another
3)Isolation - transactions don't interfere with each other: one transaction doesnt know about 
other transactions.
4)Durability - once transaction committed, it remain even after system failue..: once committed , it remains
same even after system crash

Why transaction:
*to maintain integrity and consistency

basic transaction command:
*begin
*commit
*rollback - 
*savepoint
*/
truncate table tbl_bank_accounts

create table tbl_bank_accounts
(
account_id serial primary key,  --serial : autoincrement variable
account_name varchar(30),
balance numeric(10,2)   --numeric : decimal of 10 characters
);


insert into tbl_bank_accounts (account_name,balance)
values
('Alice',5000.00),
('Bob',3000.00);

select * from tbl_bank_accounts;

--1)beginning the transactions/Tran

begin; --start of transaction

--alice to bob rs500

update tbl_bank_accounts set balance = balance - 500 where account_name = 'Alice'; 
--deduct from alice account

update tbl_bank_accounts set balance = balance + 500 where account_name = 'Bob';
--amount added to bob account


commit; --saving changes in transactions permanently
select * from tbl_bank_accounts; --alice:4500 & bob:3500 after tran


--2)scenario with error :rollback

begin;

update tbl_bank_accounts set balance = balance - 100 where account_name = 'Alice'; 
--deduct from alice account

update tbl_bank_account set balance = balance + 100 where account_name = 'Bob';
--amount added to bob account

rollback; --takes back to the last saved state
-- commit; 
select * from tbl_bank_accounts;



--3)savepoint:partial rollback
begin; --start of transaction

update tbl_bank_accounts set balance = balance - 100 where account_name = 'Alice'; 

savepoint after_alex; --savepoint savepoint_name :syntax

update tbl_bank_account set balance = balance + 100 where account_name = 'Bob';
--amount added to bob account


rollback to after_alex ; --rollback to savepoint

update tbl_bank_accounts set balance = balance + 100 where account_name = 'Bob';

commit;

select * from tbl_bank_accounts;




--savepoint :example 2
begin; --start of transaction

update tbl_bank_accounts set balance = balance - 100 where account_name = 'Alice'; 

savepoint after_alex; 

update tbl_bank_account set balance = balance + 100 where account_name = 'Bob';


rollback to after_alex ; --rollback to savepoint

update tbl_bank_accounts set balance = balance + 100 where account_name = 'Bob';

commit;

select * from tbl_bank_accounts;



-- UPDATE inside BEGIN TRAN
START/BEGIN TRANSACTION;
UPDATE tbl_bank_accounts
SET balance = balance + 500
WHERE account_name = 'Alice';

SELECT * FROM tbl_bank_accounts;
-- At this point, change is not committed yet.
COMMIT; -- Change is permanently saved.
-- Open a different psql instance and check the table records.

-- Inside BEGIN OR START TRANSACTION, nothing is auto-committed.

-- BEGIN, UPDATES, NOT COMMIT -> changes are not saved unless you do a commit.

-- This is auto-committed by default.
UPDATE tbl_bank_accounts SET balance = balance - 4500 WHERE account_name = 'Alice';

-- Auto-Commit
/*
In PSQL, autocommit is ON by default.
MySQL -> SET autocommit = 1; //Enable , set autocommit off //disable
*/

--Task to model an transaction :from image in chat
do $$
declare rows_updated int;
BEGIN
	 ---start of transation
	insert into tbl_bank_accounts (name,balance) values('john',0);

	if exists(select 1 from tbl_bank_accounts where name = 'john')
	then 
		savepoint pointA;
	else
		raise notice 'Record hasnt inserted';
		rollback;
		return; --exit do block;
	end if;

	update tbl_bank_accounts
    SET balance = balance + 500 where account_name = 'john';

	savepoint afterjohn_update;

	--deducting from alice and bob;
	update tbl_bank_accounts SET balance = balance - 300 where account_name = 'Alice';
	update tbl_bank_accounts SET balance = balance - 200 where account_name = 'Bob';

	GET DIAGNOSTICS rows_updated = ROW_COUNT;--gets affected rows
	if rows_updated != 2 then 
		raise notice 'Updation failed';
		rollback to pointA;
		return;
	end if;

	savepoint pointB;

	commit;
	raise notice 'Transaction completed successfully';
	exception when others then
    	rollback;
    	raise notice 'Error occurred. Transaction rolled back.';
end;
end $$;



abort;
---
--concurrency:
/*
POSTGRE handles concurrency using
1)MVCC (multi-version concurrency control)

2)isolation levels :4
	1)read uncommitted - not supported in postgre
	2)read commit - default
	3)repeatable read - ensures repeatable reads
	4)serializable - full isolation (safe but slow operations handling)
		-no (dirty reads,lost updates,)


Problems without proper concurrency control:
1)Inconsistent reads/Dirty read: reading uncommited data from another trans,which might disappear later.
trans A updates a row but doesnt commit yet
trans b reads updated row
trans A rolls back to update
now trans b has read data that never officialy existed - which is dirty read.


--why happen?
db runs at low isolation level(read&committed):
read & uncommitted levels - allows dirty reads
--higher isolation levels prevent dirty reads but slow!.

2)Lost update:
trans A  reads a record
trans b reads same record.
trans A updates and writes it back.
trans b still hold the old val and write its updates ,overwrites A's changes.


solution to it:
1)Perssimistic locking(explicit lock)
--lock the record when someone reads it,so no one else can read or write it until the 
lock is released.

2)optimistic locking(versioning) - common & scalable
each record has a timestamp or version number
when updatig you check the version hasnt changed since u read it.
if it changed,you reject the update(then retrying)

3)serialazable isolation level
using higher isolation levels can prevent lost updates.
but its heavier and cause performance issues.


which solution is best:

1)webapp/api : optimistic locking
2)critical financial system : pessimistic lock - safer


Inconsistent reads : also known as read anomolies
1.Dirty reads
2.non -repeatable reads.
trans A reads a row. --100
trans b updates and commits the row, then  --90
trans A reads the row again and sees diff data


3.phantom reads
new rows appear when the same query is run again

*/



abort;



create table accounts
(
id int primary key,
balance int
);

insert into accounts (id,balance) values
(1,1000);

select * from accounts;

--trans A :step2
begin transaction;
update accounts set balance = 0 where id =1;

--step3 :trans b
--dirty reads can be allowed when the uncommitted reads
set transaction isolation level read uncommitted; --allowing dirty reads
begin transaction;
select balance from accounts where id=1; --user b reads the balance of A that is '0'


--step4 :trans A decided to rollback
rollback; --after this,the balance will be 1000. but the b gets to see that balance is 0:dirty read.



--non-repeatable reads - same as dirty reads

--trans A reads val
select * from accounts;

--trans b changes it
update accounts set balance = balance + 200 where id = 1;

--trans A reads again:
select * from accounts; --reads an different values.


--phantom reads :same query gives another result when runs again
 --trans A
begin;
select * from accounts where balance > 500;

--trans b
begin;
insert into accounts (id,balance) values(2,550);
commit;

--trans A:runs same query again and sees a new records inserted;
begin;
select * from accounts where balance > 500; --new row appearred: phantom row



--lost-updates

--trans A 
begin;
select * from accounts where balance > 500;

--trans b
begin;
select * from accounts where balance > 500;

--trans a
update accounts set balance = balance - 200 where id = 1;
select * from accounts where balance > 500; --balance of a is 1000

--trans b
--now it updates in next terminal .....but in tran b balance is 1100

--we could see trans b wait till trans A gets committedl
commit;--now transb executes






---questions
1️⃣ Question:
In a transaction, if I perform multiple updates and an error happens in the third statement,
but I have not used SAVEPOINT,
what will happen if I issue a ROLLBACK?
Will my first two updates persist?
/*ANs: No,when we use rollback it goes back to last saved state. if its been partially saved, everything
is lost.
*/

2️⃣ Question:
Suppose Transaction A updates Alice’s balance but does not commit. 
Can Transaction B read the new balance if the isolation level is set to READ COMMITTED?
/* ANs: No,a transaction see only committed changes. read commit makes transaction to read only
committed data */

3️⃣ Question:
What will happen if two concurrent transactions both execute:
UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';
at the same time? Will one overwrite the other?
/* Ans: No, it doesnt overwrites... it undergoes rowl level automatically
1)in first session, when the update executes..only after the commit here, the second session updation
can be executed ..until then it waits.
2)after commit in first session, second trans gets executed..so it doesnt overwrite and 
produces an output.
*/

4️⃣ Question:
If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes made 
after the savepoint or everything?
/*Ans: undo only the changes made after the savepoint.everything before savepoint is preserved.
*/
5️⃣ Question:
Which isolation level in PostgreSQL prevents phantom reads?
--ANs: Serialazation :makes transaction as if its the only on running.

6️⃣ Question:
Can Postgres perform a dirty read (reading uncommitted data from another transaction)?
--Ans: No, postgre doesnt support it.the low isolation level is read committed.

7️⃣ Question:
If autocommit is ON (default in Postgres), and I execute an UPDATE, 
is it safe to assume the change is immediately committed?
--Ans: yes, defualtly autocommit is on.

8️⃣ Question:
If I do this:

BEGIN;
UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- (No COMMIT yet)
And from another session, I run:

SELECT balance FROM accounts WHERE id = 1;
Will the second session see the deducted balance?

/*
ANs:No,
the second session cant see the deducted balance.
only the latest committed changes will be viewed in second session 
*/
