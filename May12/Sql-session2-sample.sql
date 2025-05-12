select * from tbl_bank_accounts;
select * from accounts;

--trans b
begin;
select * from accounts where balance > 500;

UPDATE accounts SET balance = balance - 100 WHERE id = 1;
-- balance becomes 900, overwriting A's change

COMMIT;
