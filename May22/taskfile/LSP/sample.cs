using System;

/*
LSP states every subclass should follow the parent class, if anything doesnt
then it violates LSP 

example:1)
birds 
-> a bird parent class with fly method :- can fly
0> subclass :- sparrow :- can fly .yeah
-> subclass :- ostrich :- can't fly -> it violates the parent class, its a bird but cant fly.
wherease parent class states that "a bird need to fly".

2)Accounts
->withdrawing amount is an parent class just tell how much amount been withrawn.
-> where savings account,fixeddeposit account ..there are many account types!!
->so when, fixeddeposit type is been called, it violates the parent class:- because 
we cant withdraw in fd account unless it became matured....

--implementing :- EmployeeWorking hours structure
where, type of employees will be subclass to call their working hours...

*/