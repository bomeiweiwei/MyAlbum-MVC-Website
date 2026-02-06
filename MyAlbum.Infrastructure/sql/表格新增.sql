/* =========================================================
   Album MVC Practice - FINAL DDL + Seed (SQL Server)
   - Table names: singular (Account, Employee, Member, AlbumCategory, Album, AlbumPhoto, AlbumComment)
   - AccountType: 1 = Employee, 2 = Member
   - Album has OwnerAccountId (ownership)
   - AlbumComment FK -> AlbumPhoto.AlbumPhotoId
   - Album.TotalCommentNum + AlbumPhoto.CommentNum
   - Every table includes: CreatedAtUtc, UpdatedAtUtc, CreatedBy, UpdatedBy (FK -> Account)
   - Includes DROP + CREATE + SEED (admin/mark + categories)
   ========================================================= */

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRAN;

------------------------------------------------------------
-- DROP TABLES (reverse FK order)
------------------------------------------------------------
IF OBJECT_ID('dbo.AlbumComment', 'U') IS NOT NULL DROP TABLE dbo.AlbumComment;
IF OBJECT_ID('dbo.AlbumPhoto',  'U') IS NOT NULL DROP TABLE dbo.AlbumPhoto;
IF OBJECT_ID('dbo.Album',       'U') IS NOT NULL DROP TABLE dbo.Album;
IF OBJECT_ID('dbo.AlbumCategory','U') IS NOT NULL DROP TABLE dbo.AlbumCategory;
IF OBJECT_ID('dbo.Member',      'U') IS NOT NULL DROP TABLE dbo.Member;
IF OBJECT_ID('dbo.Employee',    'U') IS NOT NULL DROP TABLE dbo.Employee;
IF OBJECT_ID('dbo.Account',     'U') IS NOT NULL DROP TABLE dbo.Account;

------------------------------------------------------------
-- Account
-- AccountType: 1=Employee, 2=Member
------------------------------------------------------------
CREATE TABLE dbo.Account
(
    AccountId        UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Account PRIMARY KEY,
    UserName         NVARCHAR(50)      NOT NULL,
    PasswordHash     NVARCHAR(255)     NOT NULL,
    AccountType      TINYINT           NOT NULL,  -- 1=Employee, 2=Member
    Status           TINYINT           NOT NULL CONSTRAINT DF_Account_Status DEFAULT (1),
    LastLoginAtUtc   DATETIME2(3)      NULL,

    CreatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Account_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Account_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy        UNIQUEIDENTIFIER  NOT NULL,  -- FK added after seed (bootstrap)
    UpdatedBy        UNIQUEIDENTIFIER  NOT NULL   -- FK added after seed (bootstrap)
);

CREATE UNIQUE INDEX UX_Account_UserName ON dbo.Account(UserName);
CREATE INDEX IX_Account_AccountType_Status ON dbo.Account(AccountType, Status);

------------------------------------------------------------
-- Employee (1:1 Account)
------------------------------------------------------------
CREATE TABLE dbo.Employee
(
    EmployeeId       UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Employee PRIMARY KEY,
    AccountId        UNIQUEIDENTIFIER NOT NULL,

    Email            NVARCHAR(320)     NOT NULL,
    Phone            NVARCHAR(30)      NULL,
    Status           TINYINT           NOT NULL CONSTRAINT DF_Employee_Status DEFAULT (1),

    CreatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Employee_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Employee_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy        UNIQUEIDENTIFIER  NOT NULL,
    UpdatedBy        UNIQUEIDENTIFIER  NOT NULL,

    CONSTRAINT FK_Employee_Account     FOREIGN KEY (AccountId) REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_Employee_CreatedBy  FOREIGN KEY (CreatedBy) REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_Employee_UpdatedBy  FOREIGN KEY (UpdatedBy) REFERENCES dbo.Account(AccountId)
);

CREATE UNIQUE INDEX UX_Employee_AccountId ON dbo.Employee(AccountId);

------------------------------------------------------------
-- Member (1:1 Account)
------------------------------------------------------------
CREATE TABLE dbo.Member
(
    MemberId         UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Member PRIMARY KEY,
    AccountId        UNIQUEIDENTIFIER NOT NULL,

    Email            NVARCHAR(320)     NOT NULL,
    DisplayName      NVARCHAR(50)      NOT NULL,
    AvatarPath       NVARCHAR(260)     NULL,
    Status           TINYINT           NOT NULL CONSTRAINT DF_Member_Status DEFAULT (1),

    CreatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Member_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Member_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy        UNIQUEIDENTIFIER  NOT NULL,
    UpdatedBy        UNIQUEIDENTIFIER  NOT NULL,

    CONSTRAINT FK_Member_Account     FOREIGN KEY (AccountId) REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_Member_CreatedBy  FOREIGN KEY (CreatedBy) REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_Member_UpdatedBy  FOREIGN KEY (UpdatedBy) REFERENCES dbo.Account(AccountId)
);

CREATE UNIQUE INDEX UX_Member_AccountId ON dbo.Member(AccountId);
-- (optional) unique email:
-- CREATE UNIQUE INDEX UX_Member_Email ON dbo.Member(Email);

------------------------------------------------------------
-- AlbumCategory (admin-defined)
------------------------------------------------------------
CREATE TABLE dbo.AlbumCategory
(
    AlbumCategoryId  UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_AlbumCategory PRIMARY KEY,
    CategoryName     NVARCHAR(50)      NOT NULL,
    SortOrder        INT               NOT NULL CONSTRAINT DF_AlbumCategory_SortOrder DEFAULT (0),
    Status           TINYINT           NOT NULL CONSTRAINT DF_AlbumCategory_Status DEFAULT (1),

    CreatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_AlbumCategory_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_AlbumCategory_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy        UNIQUEIDENTIFIER  NOT NULL,
    UpdatedBy        UNIQUEIDENTIFIER  NOT NULL,

    CONSTRAINT FK_AlbumCategory_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_AlbumCategory_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES dbo.Account(AccountId)
);

CREATE UNIQUE INDEX UX_AlbumCategory_CategoryName ON dbo.AlbumCategory(CategoryName);

------------------------------------------------------------
-- Album (member-owned container)
------------------------------------------------------------
CREATE TABLE dbo.Album
(
    AlbumId          UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Album PRIMARY KEY,
    AlbumCategoryId  UNIQUEIDENTIFIER NOT NULL,
    OwnerAccountId   UNIQUEIDENTIFIER NOT NULL, -- ownership (Member's AccountId)

    Title            NVARCHAR(100)     NOT NULL,
    Description      NVARCHAR(1000)    NULL,
    CoverPath        NVARCHAR(260)     NULL,
    ReleaseTimeUtc   DATETIME2(3)      NOT NULL,
    TotalCommentNum  INT               NOT NULL CONSTRAINT DF_Album_TotalCommentNum DEFAULT (0),
    Status           TINYINT           NOT NULL CONSTRAINT DF_Album_Status DEFAULT (1),

    CreatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Album_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_Album_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy        UNIQUEIDENTIFIER  NOT NULL,
    UpdatedBy        UNIQUEIDENTIFIER  NOT NULL,

    CONSTRAINT FK_Album_AlbumCategory FOREIGN KEY (AlbumCategoryId) REFERENCES dbo.AlbumCategory(AlbumCategoryId),
    CONSTRAINT FK_Album_Owner         FOREIGN KEY (OwnerAccountId)  REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_Album_CreatedBy     FOREIGN KEY (CreatedBy)       REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_Album_UpdatedBy     FOREIGN KEY (UpdatedBy)       REFERENCES dbo.Account(AccountId),

    CONSTRAINT CK_Album_TotalCommentNum_NonNegative CHECK (TotalCommentNum >= 0)
);

CREATE INDEX IX_Album_Category_Status_ReleaseTimeUtc ON dbo.Album(AlbumCategoryId, Status, ReleaseTimeUtc DESC);
CREATE INDEX IX_Album_Owner_Status_ReleaseTimeUtc    ON dbo.Album(OwnerAccountId, Status, ReleaseTimeUtc DESC);

------------------------------------------------------------
-- AlbumPhoto (photos inside Album)
------------------------------------------------------------
CREATE TABLE dbo.AlbumPhoto
(
    AlbumPhotoId     UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_AlbumPhoto PRIMARY KEY,
    AlbumId          UNIQUEIDENTIFIER NOT NULL,

    FilePath         NVARCHAR(260)     NOT NULL,
    OriginalFileName NVARCHAR(255)     NULL,
    ContentType      NVARCHAR(100)     NULL,
    FileSizeBytes    BIGINT            NOT NULL CONSTRAINT DF_AlbumPhoto_FileSizeBytes DEFAULT (0),
    SortOrder        INT               NOT NULL CONSTRAINT DF_AlbumPhoto_SortOrder DEFAULT (0),
    CommentNum       INT               NOT NULL CONSTRAINT DF_AlbumPhoto_CommentNum DEFAULT (0),
    Status           TINYINT           NOT NULL CONSTRAINT DF_AlbumPhoto_Status DEFAULT (1),

    CreatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_AlbumPhoto_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_AlbumPhoto_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy        UNIQUEIDENTIFIER  NOT NULL,
    UpdatedBy        UNIQUEIDENTIFIER  NOT NULL,

    CONSTRAINT FK_AlbumPhoto_Album     FOREIGN KEY (AlbumId)    REFERENCES dbo.Album(AlbumId),
    CONSTRAINT FK_AlbumPhoto_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_AlbumPhoto_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES dbo.Account(AccountId),

    CONSTRAINT CK_AlbumPhoto_CommentNum_NonNegative CHECK (CommentNum >= 0)
);

CREATE INDEX IX_AlbumPhoto_Album_Status_SortOrder ON dbo.AlbumPhoto(AlbumId, Status, SortOrder);

------------------------------------------------------------
-- AlbumComment (FK -> AlbumPhoto)
------------------------------------------------------------
CREATE TABLE dbo.AlbumComment
(
    AlbumCommentId   UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_AlbumComment PRIMARY KEY,
    AlbumPhotoId     UNIQUEIDENTIFIER NOT NULL,
    MemberId         UNIQUEIDENTIFIER NOT NULL,

    Comment          NVARCHAR(2000)    NOT NULL,
    ReleaseTimeUtc   DATETIME2(3)      NOT NULL,
    IsChanged        BIT               NOT NULL CONSTRAINT DF_AlbumComment_IsChanged DEFAULT (0),
    Status           TINYINT           NOT NULL CONSTRAINT DF_AlbumComment_Status DEFAULT (1), -- 1=Visible

    CreatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_AlbumComment_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc     DATETIME2(3)      NOT NULL CONSTRAINT DF_AlbumComment_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy        UNIQUEIDENTIFIER  NOT NULL,
    UpdatedBy        UNIQUEIDENTIFIER  NOT NULL,

    CONSTRAINT FK_AlbumComment_AlbumPhoto FOREIGN KEY (AlbumPhotoId) REFERENCES dbo.AlbumPhoto(AlbumPhotoId),
    CONSTRAINT FK_AlbumComment_Member    FOREIGN KEY (MemberId)     REFERENCES dbo.Member(MemberId),
    CONSTRAINT FK_AlbumComment_CreatedBy FOREIGN KEY (CreatedBy)    REFERENCES dbo.Account(AccountId),
    CONSTRAINT FK_AlbumComment_UpdatedBy FOREIGN KEY (UpdatedBy)    REFERENCES dbo.Account(AccountId)
);

CREATE INDEX IX_AlbumComment_AlbumPhoto_Status_ReleaseTimeUtc
ON dbo.AlbumComment(AlbumPhotoId, Status, ReleaseTimeUtc DESC);

CREATE INDEX IX_AlbumComment_Member_Status_ReleaseTimeUtc
ON dbo.AlbumComment(MemberId, Status, ReleaseTimeUtc DESC);

------------------------------------------------------------
-- SEED
------------------------------------------------------------
DECLARE @pwd NVARCHAR(255) =
N'AQAAAAEAACcQAAAAEHSDs/er+DOO3+obUIyHxZl0fn/CYhgRwm7Ky+WUVFA7/x2F5PD+kXCe7DCHxp3QPw==';

DECLARE @adminAccountId UNIQUEIDENTIFIER = NEWID();
DECLARE @markAccountId  UNIQUEIDENTIFIER = NEWID();

-- admin (Employee) - CreatedBy/UpdatedBy are self
INSERT INTO dbo.Account
(
    AccountId, UserName, PasswordHash, AccountType, Status, LastLoginAtUtc,
    CreatedAtUtc, UpdatedAtUtc, CreatedBy, UpdatedBy
)
VALUES
(
    @adminAccountId, N'admin', @pwd, 1, 1, NULL,
    SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId
);

INSERT INTO dbo.Employee
(
    EmployeeId, AccountId, Email, Phone, Status,
    CreatedAtUtc, UpdatedAtUtc, CreatedBy, UpdatedBy
)
VALUES
(
    NEWID(), @adminAccountId, N'admin@example.com', NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId
);

-- mark (Member) - created by admin
INSERT INTO dbo.Account
(
    AccountId, UserName, PasswordHash, AccountType, Status, LastLoginAtUtc,
    CreatedAtUtc, UpdatedAtUtc, CreatedBy, UpdatedBy
)
VALUES
(
    @markAccountId, N'mark', @pwd, 2, 1, NULL,
    SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId
);

INSERT INTO dbo.Member
(
    MemberId, AccountId, Email, DisplayName, AvatarPath, Status,
    CreatedAtUtc, UpdatedAtUtc, CreatedBy, UpdatedBy
)
VALUES
(
    NEWID(), @markAccountId, N'mark@example.com', N'Mark', NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId
);

-- AlbumCategory seed (admin-defined)
INSERT INTO dbo.AlbumCategory
(
    AlbumCategoryId, CategoryName, SortOrder, Status,
    CreatedAtUtc, UpdatedAtUtc, CreatedBy, UpdatedBy
)
VALUES
(NEWID(), N'熱門景點', 10, 1, SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId),
(NEWID(), N'精緻美食', 20, 1, SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId),
(NEWID(), N'平價小吃', 30, 1, SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId),
(NEWID(), N'節慶活動', 40, 1, SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId),
(NEWID(), N'歷史古蹟', 50, 1, SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId),
(NEWID(), N'特色民宿', 60, 1, SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId),
(NEWID(), N'其他',   999,1, SYSUTCDATETIME(), SYSUTCDATETIME(), @adminAccountId, @adminAccountId);

------------------------------------------------------------
-- Add Account self-referencing FKs AFTER seed (bootstrap safe)
------------------------------------------------------------
ALTER TABLE dbo.Account
ADD CONSTRAINT FK_Account_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES dbo.Account(AccountId);

ALTER TABLE dbo.Account
ADD CONSTRAINT FK_Account_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES dbo.Account(AccountId);

COMMIT;

-- Quick sanity checks
SELECT UserName, AccountType, Status, CreatedAtUtc, CreatedBy FROM dbo.Account ORDER BY UserName;
SELECT CategoryName, SortOrder FROM dbo.AlbumCategory ORDER BY SortOrder;
