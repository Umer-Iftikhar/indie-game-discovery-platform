`Genres Table`
```sql
CREATE TABLE Genres (
    Id INT  AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB;
```

`Tags Table`
```sql
CREATE TABLE Tags (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
) ENGINE=InnoDB;
```

`GameTags Table`
```sql
-- Many-to-Many Join Table for Games and Tags
CREATE TABLE GameTag (
    GameId INT PRIMARY KEY,
    TagId INT PRIMARY KEYL,
    CONSTRAINT FK_GameTag_Games_GameId FOREIGN KEY (GameId) 
        REFERENCES Games (Id) ON DELETE CASCADE,
    CONSTRAINT FK_GameTag_Tags_TagId FOREIGN KEY (TagId) 
        REFERENCES Tags (Id) ON DELETE CASCADE
) ENGINE=InnoDB;
```

`ApplicationUsers Table`
```sql
-- Custom Identity User Table
CREATE TABLE AspNetUsers (
    Id VARCHAR(255) PRIMARY KEY,
    UserName VARCHAR(256) NULL,
    NormalizedUserName VARCHAR(256) NULL,
    Email VARCHAR(256) NULL,
    NormalizedEmail VARCHAR(256) NULL,
    EmailConfirmed TINYINT(1) NOT NULL,
    PasswordHash LONGTEXT NULL,
    SecurityStamp LONGTEXT NULL,
    ConcurrencyStamp LONGTEXT NULL,
    PhoneNumber LONGTEXT NULL,
    PhoneNumberConfirmed TINYINT(1) NOT NULL,
    TwoFactorEnabled TINYINT(1) NOT NULL,
    LockoutEnd DATETIME(6) NULL,
    LockoutEnabled TINYINT(1) NOT NULL,
    AccessFailedCount INT NOT NULL,

    -- Custom fields from ApplicationUser

    GithubUserName VARCHAR(100)  NULL ,
    CreatedDate DATETIME(6) NOT NULL,
) ENGINE=InnoDB;
```

`Games Table`
```sql
CREATE TABLE Games (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    ReleaseDate DATETIME(6) NOT NULL,
    Engine VARCHAR(50) NOT NULL,
    Platforms VARCHAR(50) NOT NULL,
    CoverImagePath VARCHAR(500) NOT NULL,
    DownloadLink VARCHAR(500) NOT NULL,
    IsFeatured TINYINT(1) NOT NULL DEFAULT 0,
    CreatedDate DATETIME(6) NOT NULL,
    GenreId INT NOT NULL,
    DeveloperId VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Games_Genres_GenreId FOREIGN KEY (GenreId) 
        REFERENCES Genres (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Games_AspNetUsers_DeveloperId FOREIGN KEY (DeveloperId) 
        REFERENCES AspNetUsers (Id) ON DELETE CASCADE
) ENGINE=InnoDB;
```

`Screenshots Table`
```sql
CREATE TABLE Screenshots (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ImagePath VARCHAR(500) NOT NULL,
    GameId INT NOT NULL,
    CONSTRAINT FK_Screenshots_Games_GameId FOREIGN KEY (GameId) 
        REFERENCES Games (Id) ON DELETE CASCADE
) ENGINE=InnoDB;
```

`Reviews Table`
```sql
CREATE TABLE Reviews (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Rating INT NOT NULL,
    Comment VARCHAR(1000) NOT NULL,
    ReviewDate DATETIME(6) NOT NULL,
    GameId INT NOT NULL,
    UserId VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Reviews_Games_GameId FOREIGN KEY (GameId) 
        REFERENCES Games (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_AspNetUsers_UserId FOREIGN KEY (UserId) 
        REFERENCES AspNetUsers (Id) ON DELETE CASCADE
) ENGINE=InnoDB;
```

`DownloadHistories Table`
```sql
CREATE TABLE DownloadHistories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DownloadDate DATETIME(6) NOT NULL,
    GameId INT NOT NULL,
    UserId VARCHAR(255) NOT NULL,
    CONSTRAINT FK_DownloadHistories_Games_GameId FOREIGN KEY (GameId) 
        REFERENCES Games (Id) ON DELETE CASCADE,
    CONSTRAINT FK_DownloadHistories_AspNetUsers_UserId FOREIGN KEY (UserId) 
        REFERENCES AspNetUsers (Id) ON DELETE CASCADE
) ENGINE=InnoDB;
```

`Wishlists Table`
```sql
CREATE TABLE Wishlists (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CreatedDate DATETIME(6) NOT NULL,
    GameId INT NOT NULL,
    UserId VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Wishlists_Games_GameId FOREIGN KEY (GameId) 
        REFERENCES Games (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Wishlists_AspNetUsers_UserId FOREIGN KEY (UserId) 
        REFERENCES AspNetUsers (Id) ON DELETE CASCADE
) ENGINE=InnoDB;
```