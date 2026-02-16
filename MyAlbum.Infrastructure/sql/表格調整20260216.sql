ALTER TABLE dbo.Account DROP CONSTRAINT FK_Account_CreatedBy;
ALTER TABLE dbo.Account DROP CONSTRAINT FK_Account_UpdatedBy;

ALTER TABLE dbo.Employee DROP CONSTRAINT FK_Employee_CreatedBy;
ALTER TABLE dbo.Employee DROP CONSTRAINT FK_Employee_UpdatedBy;

ALTER TABLE dbo.Member DROP CONSTRAINT FK_Member_CreatedBy;
ALTER TABLE dbo.Member DROP CONSTRAINT FK_Member_UpdatedBy;

ALTER TABLE dbo.AlbumCategory DROP CONSTRAINT FK_AlbumCategory_CreatedBy;
ALTER TABLE dbo.AlbumCategory DROP CONSTRAINT FK_AlbumCategory_UpdatedBy;

ALTER TABLE dbo.Album DROP CONSTRAINT FK_Album_CreatedBy;
ALTER TABLE dbo.Album DROP CONSTRAINT FK_Album_UpdatedBy;

ALTER TABLE dbo.AlbumPhoto DROP CONSTRAINT FK_AlbumPhoto_CreatedBy;
ALTER TABLE dbo.AlbumPhoto DROP CONSTRAINT FK_AlbumPhoto_UpdatedBy;

ALTER TABLE dbo.AlbumComment DROP CONSTRAINT FK_AlbumComment_CreatedBy;
ALTER TABLE dbo.AlbumComment DROP CONSTRAINT FK_AlbumComment_UpdatedBy;
