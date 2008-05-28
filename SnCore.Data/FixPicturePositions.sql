-- 2008-05-27: fix duplicate positions of pictures by resetting indexes to zero

-- place pictures
UPDATE PlacePicture SET [Position] = 0 
WHERE Place_Id IN (
 SELECT DISTINCT p1.Place_Id FROM PlacePicture p1, PlacePicture p2 
 WHERE p1.Place_Id = p2.Place_Id 
 AND p1.PlacePicture_Id != p2.PlacePicture_Id
 AND p1.Position = p2.Position
 AND p1.Position != 0
)

-- account pictures
UPDATE AccountPicture SET [Position] = 0 
WHERE Account_Id IN (
 SELECT DISTINCT a1.Account_Id FROM AccountPicture a1, AccountPicture a2 
 WHERE a1.Account_Id = a2.Account_Id 
 AND a1.AccountPicture_Id != a2.AccountPicture_Id
 AND a1.Position = a2.Position
 AND a1.Position != 0
)

-- account event pictures
UPDATE AccountEventPicture SET [Position] = 0 
WHERE AccountEvent_Id IN (
 SELECT DISTINCT e1.AccountEvent_Id FROM AccountEventPicture e1, AccountEventPicture e2 
 WHERE e1.AccountEvent_Id = e2.AccountEvent_Id 
 AND e1.AccountEventPicture_Id != e2.AccountEventPicture_Id
 AND e1.Position = e2.Position
 AND e1.Position != 0
)

-- account story pictures
UPDATE AccountStoryPicture SET [Position] = 0 
WHERE AccountStory_Id IN (
 SELECT DISTINCT s1.AccountStory_Id FROM AccountStoryPicture s1, AccountStoryPicture s2 
 WHERE s1.AccountStory_Id = s2.AccountStory_Id 
 AND s1.AccountStoryPicture_Id != s2.AccountStoryPicture_Id
 AND s1.Position = s2.Position
 AND s1.Position != 0
)

-- account group pictures
UPDATE AccountGroupPicture SET [Position] = 0 
WHERE AccountGroup_Id IN (
 SELECT DISTINCT g1.AccountGroup_Id FROM AccountGroupPicture g1, AccountGroupPicture g2 
 WHERE g1.AccountGroup_Id = g2.AccountGroup_Id 
 AND g1.AccountGroupPicture_Id != g2.AccountGroupPicture_Id
 AND g1.Position = g2.Position
 AND g1.Position != 0
)
