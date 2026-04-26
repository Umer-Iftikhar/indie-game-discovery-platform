use indiegameplatform;
-- --------------- Admin Ka Kaam --------------

SELECT g.Title, COUNT(*) as WishlistCount
FROM Wishlists w
INNER JOIN Games g ON w.GameId = g.Id
GROUP BY g.Title
ORDER BY WishlistCount DESC
LIMIT 1;

SELECT 
    r.Name AS RoleName, 
    COUNT(ur.UserId) AS TotalUsers
FROM AspNetRoles r
INNER JOIN AspNetUserRoles ur ON r.Id = ur.RoleId
INNER JOIN AspNetUsers u ON ur.UserId = u.Id
GROUP BY r.Name;