/****** Object:  Table [dbo].[ErrorLog]    Script Date: 7/14/2021 12:50:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[ErrorLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[AddedDate] [datetime] NULL,
	[LineNumber] [int] NULL,
	[Message] [nvarchar](max) NULL,
	[ErrorNumber] [int] NULL,
	[ProcedureName] [nvarchar](256) NULL,
	[Severity] [int] NULL,
	[State] [int] NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[ErrorLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ItemMaster]    Script Date: 7/14/2021 12:50:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Description] [varchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[ImgURL] [varchar](500) NULL,
 CONSTRAINT [PK_ItemMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_AddedDate]  DEFAULT (getdate()) FOR [AddedDate]
GO

/****** Object:  StoredProcedure [dbo].[usp_LogError]    Script Date: 7/14/2021 12:50:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Surjitsingh Tuteja
-- Description:	Log the error
-- =============================================
CREATE PROCEDURE [dbo].[usp_LogError]
	@ErrorLogId BIGINT = NULL OUT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message NVARCHAR(MAX),
			@Severity INT,
			@State INT
	SELECT @Message = ERROR_MESSAGE(), @Severity = ERROR_SEVERITY(), @State = ERROR_STATE()
	BEGIN TRY
		INSERT INTO ErrorLog (
			LineNumber
			, [Message]
			, ErrorNumber
			, ProcedureName
			, Severity
			, [State]
		)
		SELECT 
			ERROR_LINE()
			, @Message
			, ERROR_NUMBER()
			, ERROR_PROCEDURE()
			, @Severity
			, @State;

		SET @ErrorLogId = SCOPE_IDENTITY();
		
		SET NOCOUNT OFF;
		RETURN;
	END TRY
	BEGIN CATCH
		SELECT ERROR_NUMBER() AS [ERRORNumber], ERROR_MESSAGE() AS ERRORMessage, ERROR_LINE() AS ERRORLine;
		THROW 6000, 'Error in Insert records in ErrorLog table', 1;
	END CATCH    
END
GO

/****** Object:  StoredProcedure [dbo].[usp_AddUpdateItem]    Script Date: 7/14/2021 12:50:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author: Surjitsingh Tuteja>
-- Create date: <Create Date: 10 July 2021>
-- Description:	<Description: Add Update Item>
-- =============================================
CREATE PROCEDURE [dbo].[usp_AddUpdateItem]
	@Id				INT OUTPUT   
    , @Name			VARCHAR(50)
	, @Price		DECIMAL(18, 2)
    , @Description	VARCHAR(500)
	, @ImgURL		VARCHAR(500)
AS
BEGIN	
	SET NOCOUNT ON;	
	SET XACT_ABORT ON;

	DECLARE @CurrentDate DATETIME = GETDATE();
	
	BEGIN TRANSACTION
	BEGIN TRY
		
		IF (@Id = 0) 
		BEGIN
			INSERT INTO [dbo].[ItemMaster]
			(
				[Name]
				, Price
				, [Description]
				, CreatedDate
				, ImgURL
			)
			VALUES
			(
				@Name
				, @Price
				, @Description
				, @CurrentDate
				, @ImgURL
			)			

			SET @Id = SCOPE_IDENTITY();
		END
		ELSE
		BEGIN
			UPDATE 
				[dbo].[ItemMaster]
			SET				
				[Name] = @Name
				, Price = @Price
				, [Description] = @Description
				, [ImgURL] = @ImgURL
				, LastModifiedDate = @CurrentDate
			WHERE
				Id = @Id			
		END
		

		IF (XACT_STATE() = 1) BEGIN
			COMMIT TRANSACTION;
		END

		RETURN;			
	END TRY
	BEGIN CATCH
		SET @Id = 0;
		
		IF (XACT_STATE() = -1) BEGIN
			ROLLBACK TRANSACTION;
		END

		DECLARE @ErrorLogId BIGINT;

		EXEC [dbo].[usp_LogError] @ErrorLogId = @ErrorLogId OUT;

		SELECT @ErrorLogId AS [ErrorLogId];

		RETURN;
	END CATCH
END
GO

/****** Object:  StoredProcedure [dbo].[usp_GetItems]    Script Date: 7/14/2021 12:50:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author: Surjitsingh Tuteja>
-- Create date: <Create Date: 12 July 2021>
-- Description:	<Description: Get Items>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetItems]
	@Keyword			VARCHAR(50)
	, @PageNo			INT = 1
	, @PageSize			INT = 100
	, @SortField		VARCHAR(20) = 'Name'
	, @SortExp			VARCHAR(20) = 'asc'
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
		
	IF (@PageNo = 0 OR @PageNo IS NULL) SET @PageNo = 1;
	IF (@PageSize = 0 OR @PageSize IS NULL) SET @PageSize = 10;
	IF (@SortField NOT IN ('Name', 'Price')) SET @SortField = 'Name';
	IF (@SortExp NOT IN ('asc', 'desc')) SET @SortField = 'asc';

	SET @Keyword = IIF(@Keyword IS NULL, NULL, LTRIM(RTRIM(@Keyword)));

	BEGIN TRY		
		; WITH CTE_Results AS
		(  
			SELECT 
				COUNT(1) OVER() AS [TotalRows]
				, Id
				, ImgURL
				, [Name]
				, Price
				, [Description]
				, CreatedDate				
			FROM 
				dbo.ItemMaster
			WHERE 
				1 = CASE WHEN @Keyword IS NULL THEN 1
						 WHEN [Name] LIKE '%' + @Keyword + '%' OR [Description] LIKE '%' + @Keyword + '%' THEN 1 
					ELSE 0 END
			ORDER BY  
				CASE WHEN (@SortField = 'Name' AND @SortExp = 'asc') THEN [Name] END ASC,
				CASE WHEN (@SortField = 'Name' AND @SortExp = 'desc') THEN [Name] END DESC,				
				CASE WHEN (@SortField = 'Price' AND @SortExp = 'asc') THEN Price END ASC,
				CASE WHEN (@SortField = 'Price' AND @SortExp = 'desc') THEN Price END DESC

				OFFSET @PageSize * (@PageNo - 1) ROWS  
				FETCH NEXT @PageSize ROWS ONLY  			
		)
		SELECT 
				TotalRows, Id, ImgURL, [Name], Price, [Description], CreatedDate
		FROM 
			CTE_Results

	END TRY
	BEGIN CATCH
		DECLARE @ErrorLogId BIGINT;
		EXEC [dbo].[usp_LogError] @ErrorLogId = @ErrorLogId OUT;
		SELECT @ErrorLogId AS [ErrorLogId];
		THROW;
	END CATCH
END
GO