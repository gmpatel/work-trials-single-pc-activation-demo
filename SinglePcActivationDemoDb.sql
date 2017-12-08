Select * From [User]


-- Run this query to simulate make users email address not verified
-- update [user] set IsEmailVerified = 0

-- Run this query to simulate make users email address verified
-- update [user] set IsEmailVerified = 1

-- Run this query to simulate allow users to activate the app again on new machine again
-- update [user] set Activated = 0, ActivatedMachineKey = null, ActivatedDateTime = null

-- Run this query to simulate user already has activated app from one machine and trying to activate/run/login from another machine
-- update [user] set ActivatedMachineKey = 'B31RY52|S1EVNSAG404923|BFEBFBFF000306D4|4C:34:88:10:61:A3'
