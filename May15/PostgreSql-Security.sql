--May15 - Postgresql security:- encrytption,decryption,masking 


/*
1. Create a stored procedure to encrypt a given text
Task: Write a stored procedure sp_encrypt_text that takes a plain text input 
(e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
 
Use pgp_sym_encrypt(text, key) from pgcrypto.

pgcrypto :- used to encrypt/decrypt
pgp_sym_encrypt(text, key) :- takes plain text and with symmetric key(password).

*/
CREATE EXTENSION IF NOT EXISTS pgcrypto; --checking if pgcrypto exists;


--now use pgp_sym_encrypt(text, key) to encrypt data
create or replace function fn_encrypt_text(in plain_email text, secret_key text )
returns bytea as $$
begin
	return pgp_sym_encrypt(plain_email, secret_key);
end;
$$ language plpgsql;
--function created which is to encrypt an data with an secret binary key(password type)

select fn_encrypt_text('dharun@gmail.com','q!w@e#r$t%y^');
select encode(fn_encrypt_text('dharun@gmail.com','q!w@e#r$t%y^'),'base64');


create or replace procedure proc_encrypt_data(
    in input_text text,
    in secret_key text,
    out encrypted_result bytea
)
as $$
begin
    encrypted_result := pgp_sym_encrypt(input_text, secret_key);
end;
$$ language plpgsql;

do $$
declare
    encrypted_val bytea;
begin
    call proc_encrypt_data('dharun@gmail.com','q!w@e#r$t%y^', encrypted_val);
    raise notice 'Encrypted value: %', encode(encrypted_val, 'base64');  
end;
$$;



/*2. Create a stored procedure to compare two encrypted texts
Task: Write a procedure sp_compare_encrypted that takes two encrypted
values and checks if they decrypt to the same plain text.

*/
--comapring if 2 encrypted emails are same!

--we will first decrypting the encrypted data by ( pgp_sym_decrypt(text,key) ).
create or replace function fn_compare(email1 bytea,email2 bytea,secret_key text)
returns boolean as $$
declare
	decrypt_text1 text;
	decrypt_text2 text;
begin
	decrypt_text1 := pgp_sym_decrypt(email1,secret_key);
	decrypt_text2 := pgp_sym_decrypt(email2,secret_key);

	if decrypt_text1 = decrypt_text2 then
		return true;
	else
		return false;
	end if;
end;
$$ language plpgsql;


/*here the plain value is first encrypted with use of encryption function while passing here as paramter,
then it is been decrypted using the extension . and finally it checks with both same plain text(decrypted-one)
whether it true or false.
*/

select fn_compare(fn_encrypt_text('dharun@gmail.com','q!w@e#r$t%y^'), --returns true as no mistake.
fn_encrypt_text('dharun@gmail.com','q!w@e#r$t%y^'),
'q!w@e#r$t%y^');

select fn_compare(fn_encrypt_text('dharun@gmail.com','q!w@e#r$t%y^'), --returns false as there is a mistake.
fn_encrypt_text('dharn@gmail.com','q!w@e#r$t%y^'),
'q!w@e#r$t%y^');


create or replace procedure proc_compare(
    in name1 bytea,
    in name2 bytea,
    in secret_key text,
    out is_equal boolean
)
as $$
declare 
    decrypted_name1 text;
    decrypted_name2 text;
begin
    decrypted_name1 := pgp_sym_decrypt(name1, secret_key);
    decrypted_name2 := pgp_sym_decrypt(name2, secret_key);

    is_equal := (decrypted_name1 = decrypted_name2);
end;
$$ language plpgsql;


do $$
declare
    is_same boolean;
	enc1 bytea;
	enc2 bytea;
begin
	call proc_encrypt_data('dharun@gmail.com','q!w@e#r$t%y^', enc1);
	call proc_encrypt_data('dharun@gmail.com','q!w@e#r$t%y^', enc2);
    call proc_compare_encrypted_texts(enc1,enc2, 'q!w@e#r$t%y^',is_same);
    raise notice 'Same or Not: %', is_same; 
end;
$$;

/*3. Create a stored procedure to partially mask a given text
Task: Write a procedure sp_mask_text that:
 
Shows only the first 2 and last 2 characters of the input string
 
Masks the rest with *
 
E.g., input: 'john.doe@example.com' → output: 'jo***************om'

*/

create or replace function fn_mask_text(input_text text)
returns text as $$
declare
	masked text;
	length int := length(input_text); --takes length of input
begin
	if length <= 4 then
		return repeat('*',length);
	end if;
	masked:= substring(input_text,1,2) || repeat('*',length-4) || substring(input_text,length-1,2); --using substring function print first and last 2 characters.
	/*
	another method for masking:-
	left(input_text,2) || repeat('*',length-4) || right(input_text,2);
	*/

	return masked;
end;
$$ language plpgsql;

select fn_mask_text('Ram');

select fn_mask_text('computer');

select fn_mask_text('dharun@gmail.com');

--procedure
create or replace procedure proc_masking(
    in name text,
    out masked_name text
)
as $$
begin
    if length(name) <= 4 then
        masked_name := name;
    else
        masked_name := left(name, 2) || repeat('*', length(name) - 4) || right(name, 2);
    end if;
end;
$$ language plpgsql;


do $$
declare
    result text;
begin
    call proc_masking('computer', result);
    raise notice 'Masked Name: %', result;
end;
$$;


/*
4. Create a procedure to insert into customer with encrypted email and masked name
Task:
 
Call sp_encrypt_text for email
 
Call sp_mask_text for first_name
 
Insert masked and encrypted values into the customer table
 
Use any valid address_id and store_id to satisfy FK constraints.
 */


 CREATE TABLE customer (
    customer_id SERIAL PRIMARY KEY,
    first_name text,
	last_name text,
    email BYTEA,
    address_id int,
    store_id int,
	updated_date timestamp default current_timestamp
);


create or replace procedure sp_insert_customer(p_first_name text,
p_last_name text,p_email text,p_address_id int,p_store_id int)
language plpgsql
as $$
declare
	encrypt_mail bytea;
	mask_fname text;
	mask_lname text;
	secret text := 'q!w@e#r$t%y^';
begin
	encrypt_mail := fn_encrypt_text(p_email,secret);
	mask_fname := fn_mask_text(p_first_name);
	mask_lname := fn_mask_text(p_last_name);
	
	insert into customer (first_name,last_name,email,address_id,store_id)
	values
	(mask_fname,mask_lname,encrypt_mail,p_address_id,p_store_id);
end;
$$;

call sp_insert_customer('John','sebastian',
'john@sebastian@gmail.com',1,1);

call sp_insert_customer('Albert','jackson','abj@gmail.com',2,2);

select * from customer;



/*
5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
Task:
Write sp_read_customer_masked() that:
 
Loops through all rows
 
Decrypts email
 
Displays customer_id, masked first name, and decrypted email
*/

create or replace procedure sp_read_customer_masked(secret_key text)
as $$
declare	
	rec record;
	cur cursor for select * from customer;
	decrypt_mail text;
begin
	open cur;

	loop
		fetch cur into rec;
		exit when not found;

		decrypt_mail := pgp_sym_decrypt(rec.email,secret_key);

		raise notice 'customer_id :% ,first_name : % ,Decrypted_email : % ',rec.customer_id,
			rec.first_name,decrypt_mail;
	end loop;

	close cur;
end;
$$ language plpgsql;

call sp_read_customer_masked('q!w@e#r$t%y^');


