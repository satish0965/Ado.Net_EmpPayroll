---Welcome To Employee Pay Roll Data Base Problem---

--UC1-- Create Payroll_Service Database
create database Payroll_Service1;
use Payroll_Service1;

--UC2-- employee_payroll table
Create Table employee_payroll1(
Id int identity (1,1) primary key,
Name varchar(200),
Salary float,
StartDate date);

--UC3-- Create employee_payroll data as part of CURD Operation
insert into employee_payroll1 (Name, Salary, StartDate) values
('Satish', 70000.00, '2007-02-03'),
('Pritish', 50000.00, '2010-05-04'),
('Amit', 60000.00, '2009-06-09'),
('Lochan', 40000.00, '2018-03-05'),
('Akshata', 50000.00, '2020-08-02');

--UC4-- Retrieve employee_payroll data
select * from employee_payroll1;

--UC5-- Retrieve salary of particular employee and particular date range
select salary from employee_payroll1 where Name = 'Amit';
select * from employee_payroll1 where StartDate between cast ('2018-01-01' as date) and GETDATE();

--UC6-- add Gender to Employee_Payroll Table and Update the Rows to reflect the correct Employee Gender
alter table employee_payroll1 add Gender char(1);
update employee_payroll1 set Gender = 'M' where Id in (1,2,3);
update employee_payroll1 set Gender = 'F' where Id in (4,5);

--UC7-- find sum, average, min, max and number of male and female employees
select sum(Salary) as sumsalary,Gender from employee_payroll1 group by Gender;
select avg(Salary) as avgsalary,Gender from employee_payroll1 group by Gender; 
select max(Salary) as maxsalary,Gender from employee_payroll1 group by Gender; 
select min(Salary) as minsalary,Gender from employee_payroll1 group by Gender; 
select count(Name) as EmployeeCount,Gender from employee_payroll1 group by Gender;

--UC8-- add employee phone, department(not null), Address (using default values)
select * from employee_payroll1;
alter table employee_payroll1 add Phone bigint;
update employee_payroll1 set Phone = 9587463210; 
update employee_payroll1 set Phone = 9547863210 where ID IN (2,4); 
alter table employee_payroll1 add Address varchar(100) not null default 'Mumbai';
alter table employee_payroll1 add Department varchar(250) default 'IT';

--UC9-- Extend table to have Basic Pay, Deductions, Taxable Pay, Income Tax, Net Pay.
exec sp_rename 'employee_payroll1.salary','Basic_pay','column';  --Existing Store Procedure in sql server 
alter table employee_payroll1 add 
Deductions float not null default 0.00,
Taxable_Pay float not null default 0.00, 
Income_Tax float not null default 0.00,
Net_Pay float not null default 0.00;
update employee_payroll1 set Net_Pay = (Basic_Pay-Deductions-Taxable_Pay-Income_Tax);
select * from employee_payroll1;

--UC10-- Two departments for same employee
insert into employee_payroll1 (Name, Basic_pay, StartDate, Gender, Phone, Address, Department, Deductions, Taxable_Pay, Income_Tax,Net_pay)
values('Amit', 50000.00, '2011-05-05', 'M', 9876451230, 'Mumbai', 'TCS',2000.00, 5400.00, 1000.00,null);
update employee_payroll1 set Net_Pay = (Basic_Pay-Deductions-Taxable_Pay-Income_Tax);

--Stored procedure--
create procedure spAddEmployees
@Name varchar(100),
@Startdate Date,
@Gender char(1),
@Phone bigint,
@Department varchar(100),
@Address varchar(100),
@Basic_Pay float,
@Deductions float,
@Taxable_pay float,
@Income_tax float,
@Net_pay float
as
insert into employee_payroll1 (Name, Startdate, Gender, Phone, Address, Department, Basic_Pay, Deductions, Taxable_pay, Income_tax, Net_pay)
	values(@Name, @Startdate, @Gender, @Phone, @Address, @Department, @Basic_Pay, @Deductions, @Taxable_pay, @Income_tax, @Net_pay);

--Update basicPay
create procedure spUpdateEmployee
@Name varchar(100),
@Id int,
@Basic_Pay float
as
update employee_payroll1 set Basic_pay = @Basic_Pay where Id=@Id and Name= @Name;

exec spUpdateEmployee 'Akshata' , 10 ,78943;


--delete employee
--UC_4
create procedure spDeleteEmployee
@Name varchar(100),
@Id int
as
delete from employee_payroll1 where Id=@Id and Name = @Name;

exec spDeleteEmployee 'Akshata',10 ;