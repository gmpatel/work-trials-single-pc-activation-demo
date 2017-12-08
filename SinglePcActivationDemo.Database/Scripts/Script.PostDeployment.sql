/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists (select * from [User] where UserEmail = 'gmpat4u@gmail.com')
begin
	insert into [User] (UserEmail, UserPassword) values ('gmpat4u@gmail.com', 'D4D57BCBF10FD6257FA7FC821F5FD136')
end