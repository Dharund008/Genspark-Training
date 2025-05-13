/*
Locking mechanism

--postgre automatically apply locks, but can also be added manually

Locking is a mechanism used by SQL databases to control access to data by multiple users at the same time.
It helps to maintain data consistency, isolation, and integrity when 
multiple transactions are happening simultaneously.

types of locks:
1)row level - locks specific rows

SELECT ... FOR UPDATE — Locks rows for update, blocks others from changing them.
SELECT ... FOR SHARE — Locks rows for reading, others can still read but not update.

2)table level - Locks an entire table. Used automatically or manually.

3)advisory locks - user defined custom locks 
-manually control access to resources




mvcc vs locks
- mvcc allows readers and writers to work together without blocking.
locks are needed when multiple users try to access same data.


rules:
1.readers dont block each other.
2.writers block other writers on same row.



row-level locking 
-two users updating the row

*/

--transA
begin;
update products
set price = 500 where id = 1;
--trans A holds a lock on row id =1

--transB
begin;
update products set price = 600 where id = 1;

--now result: B waits until A commits and execute or rollbacks. : row level locking



--table level locks
1.access Share
--allows reads and writes 

begin;
lock table accounts in access share mode;
--allows other selects, even allows wrtite at same time.

2.row share
--allow select ...for updates, both reads and write but blocks row lock
begin;
lock table accounts in row share mode;

3.exclusive
--blocks write(insert/update/del) but allows reads(select)

begin;
lock table accounts in exclusive mode;

4.access exclusive:
--most aggressive lock
--locks everything, used by alter table,drop, truncate.
--internally used by ddl commands.


--A
begin;
lock table accounts in access exclusive mode;
--table is fully locked,

--B
select * from accounts;
--B will wait until A commits or rollbacks.



--Explicit row locks -> select --- for update
--sitaution : lock row before updating, avoid conflicts

--A
begin;
select * from accounts where id =1 for update;
--locked row for update : row id=1 is locked.

--B
begin;
update accounts set balance = balance + 100 where id = 1;
--now it gets wait /block until A commits/rollbacks;


--select -- for update locks the row before getting to update (midway)
--Banking, Ticket Booking, Inventory Management Systems



/*
A deadlock happens when:
Transaction A waits for B
Transaction B waits for A
They both wait forever.

-- Trans A
*/
BEGIN;
UPDATE products
SET price = 500
WHERE id = 1;
-- A locks row 1

-- Trans B
BEGIN;
UPDATE products
SET price = 600
WHERE id = 2;
-- B locks row 2

-- Trans A
UPDATE products
SET price = 500
WHERE id = 2;
-- A locks row 2 (already locked by B)

-- Trans B
UPDATE products
SET price = 600
WHERE id = 1
--B locks row 1 (already locked by A)

/*
psql detects a deadlock
ERROR: deadlock detected
It automatically aborts a transaction to resolve deadlock.
*/




-- Advisory Lock / Custom Locks
-- Get a lock with ID 12345
SELECT pg_advisory_lock(12345);
-- critical ops

-- Releas the lock
SELECT pg_advisory_unlock(12345);
-- 13 May 2025 - Task
-- 1. Try two concurrent updates to same row → see lock in action.
-- 2. Write a query using SELECT...FOR UPDATE and check how it locks row.
-- 3. Intentionally create a deadlock and observe PostgreSQL cancel one transaction.
-- 4. Use pg_locks query to monitor active locks.
-- 5. Explore about Lock Modes.
