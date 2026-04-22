use indiegameplatform;

-- Display 10 games on each page , along with their genre, Develper and their average ratings per game.
Select g.Title, g.Price, g.ReleaseDate, gen.Name, COALESCE(AVG(re.Rating), 0) AS AverageRatings, g.Id, g.CoverImagePath,
-- COALESCE handles null exceptions when there are no reviews
( -- subquery returns dev name
	select u.UserName
    from  aspnetusers u
    where u.Id = g.DeveloperId
) AS Developer
from games g 
left Join genres gen ON g.GenreId = gen.Id
Left Join reviews re  ON g.Id = re.GameId 
GROUP BY g.Title, g.Price, g.ReleaseDate, gen.Name, g.DeveloperId, g.Id, g.CoverImagePath
limit 10 -- only 10 games per page
offset 10; -- skip 10 games on next page
			