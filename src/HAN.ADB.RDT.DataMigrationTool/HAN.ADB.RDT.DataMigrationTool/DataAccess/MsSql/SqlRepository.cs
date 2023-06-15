using Dapper;
using System.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using static Dapper.SqlMapper;
using System.Data;
using System.Text;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql
{
	public class SqlRepository : ISqlRepository 
	{
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
		{
            _connectionString = connectionString;

            CreateIndexes();
		}

        public async Task<string> GetPosts(int startId)
        {
            string query = $@"
                SELECT TOP 1000
                    [Posts].[Id],
                    [Posts].[Title],
                    [Posts].[Body],
                    [Posts].[ClosedDate],
                    [Posts].[CommunityOwnedDate],
                    [Posts].[CreationDate],
                    [Posts].[FavoriteCount],
                    [Posts].[LastActivityDate],
                    (
                        SELECT [Type] 
                        FROM [PostTypes] AS [PostTypes] 
                        WHERE [Id] = [Posts].[PostTypeId]
                    ) AS [PostType],
                    [Posts].[Score],
                    (
                        JSON_QUERY('[' + REPLACE(REPLACE(REPLACE([Posts].[Tags],'<','""'),'>','""'),'""""','"",""') + ']')
                    ) AS [Tags],
                    [Posts].[ViewCount],
                    (
                        SELECT [Comments].[Id], 
                            [Comments].[Text] AS [Body], 
                            [Comments].[Score], 
                            [Comments].[CreationDate],
                            [Users].[Id] AS [User.Id],
                            [Users].[DisplayName] AS [User.DisplayName]
                        FROM [Comments] AS [Comments]
                        LEFT JOIN [Users] AS [Users]
                            ON [Comments].[UserId] = [Users].[Id]
                        WHERE [PostId] = [Posts].[Id] 
                        FOR JSON PATH
                    ) AS [Comments],
                    (
                        SELECT [Votes].[Id], 
                            [Votes].[BountyAmount], 
                            [Votes].[CreationDate],
                            [VoteTypes].[Name] AS [VoteType],
                            [Users].[Id] AS [User.Id],
                            [Users].[DisplayName] AS [User.DisplayName]
                        FROM [Votes] AS [Votes] 
                        LEFT JOIN [VoteTypes] AS [VoteTypes]
                            ON [Votes].[VoteTypeId] = [VoteTypes].[Id]
                        LEFT JOIN [Users] AS [Users]
                            ON [Votes].[UserId] = [Users].[Id]
                        WHERE [PostId] = [Posts].[Id] 
                        FOR JSON PATH
                    ) AS [Votes],
                    [PostLastEditorUsers].[Id] AS [LastEditorUser.Id],
                    [PostLastEditorUsers].[DisplayName] AS [LastEditorUser.DisplayName],
                    [PostOwnerUsers].[Id] AS [OwnerUser.Id],
                    [PostOwnerUsers].[DisplayName] AS [OwnerUser.DisplayName],
                    (
                        SELECT [Answers].[Id],
                            [Answers].[Body],
                            [Answers].[ClosedDate],
                            [Answers].[CommunityOwnedDate],
                            [Answers].[CreationDate],
                            [Answers].[FavoriteCount],
                            [Answers].[LastActivityDate],
                            [Answers].[LastEditDate],
                            (
                                SELECT [Type] 
                                FROM [PostTypes] AS [PostTypes] 
                                WHERE [Id] = [Answers].[PostTypeId]
                            ) AS [PostType],
                            [Answers].[Score],
                            (
                                SELECT [Comments].[Id], 
                                    [Comments].[Text] AS [Body], 
                                    [Comments].[Score], 
                                    [Comments].[CreationDate],
                                    [Users].[Id] AS [User.Id],
                                    [Users].[DisplayName] AS [User.DisplayName]
                                FROM [Comments] AS [Comments]
                                LEFT JOIN [Users] AS [Users]
                                    ON [Comments].[UserId] = [Users].[Id]
                                WHERE [PostId] = [Answers].[Id] 
                                FOR JSON PATH
                            ) AS [Comments],
                            (
                                SELECT [Votes].[Id], 
                                    [Votes].[BountyAmount], 
                                    [Votes].[CreationDate],
                                    [VoteTypes].[Name] AS [VoteType],
                                    [Users].[Id] AS [User.Id],
                                    [Users].[DisplayName] AS [User.DisplayName]
                                FROM [Votes] AS [Votes] 
                                LEFT JOIN [VoteTypes] AS [VoteTypes]
                                    ON [Votes].[VoteTypeId] = [VoteTypes].[Id]
                                LEFT JOIN [Users] AS [Users]
                                    ON [Votes].[UserId] = [Users].[Id]
                                WHERE [PostId] = [Answers].[Id] 
                                FOR JSON PATH
                            ) AS [Votes],
                            [PostAnswerLastEditorUsers].[Id] AS [LastEditorUser.Id],
                            [PostAnswerLastEditorUsers].[DisplayName] AS [LastEditorUser.DisplayName],
                            [PostAnswerOwnerUsers].[Id] AS [OwnerUser.Id],
                            [PostAnswerOwnerUsers].[DisplayName] AS [OwnerUser.DisplayName]
                        FROM [Posts] AS [Answers]
                        LEFT JOIN [Users] AS [PostAnswerLastEditorUsers]
                            ON [PostAnswerLastEditorUsers].[Id] = [Answers].[LastEditorUserId]
                        LEFT JOIN [Users] AS [PostAnswerOwnerUsers]
                            ON [PostAnswerOwnerUsers].[Id] = [Answers].[OwnerUserId]
                        WHERE [Answers].[ParentId] = [Posts].[Id]
                        FOR JSON PATH
                    ) AS [Answers],
                    [Posts].[AcceptedAnswerId],
                    (
                        SELECT [LinkedPosts].[Id],
                            [LinkedPosts].[Title],
                            [LinkedPosts].[Score]
                        FROM [Posts] AS [LinkedPosts]
                        INNER JOIN [PostLinks] AS [PostLinks]
                            ON [PostLinks].[PostId] = [Posts].[Id]
                            AND [PostLinks].[RelatedPostId] = [LinkedPosts].[Id]
                            AND [PostLinks].[LinkTypeId] = 1 -- Related
                        FOR JSON PATH
                    ) AS [RelatedPosts],
                    (
                        SELECT [LinkedPosts].[Id],
                            [LinkedPosts].[Title],
                            [LinkedPosts].[Score]
                        FROM [Posts] AS [LinkedPosts]
                        INNER JOIN [PostLinks] AS [PostLinks]
                            ON [PostLinks].[RelatedPostId] = [LinkedPosts].[Id]
                            AND [PostLinks].[PostId] = [Posts].[Id]
                            AND [PostLinks].[LinkTypeId] = 3 -- Duplicate
                        FOR JSON PATH
                    ) AS [DuplicatePosts]
                FROM [Posts] AS [Posts]
                LEFT JOIN [Users] AS [PostLastEditorUsers]
                    ON [PostLastEditorUsers].[Id] = [Posts].[LastEditorUserId]
                LEFT JOIN [Users] AS [PostOwnerUsers]
                    ON [PostOwnerUsers].[Id] = [Posts].[OwnerUserId]
                WHERE [Posts].[ParentId] IS NULL
                AND [Posts].[Id] > @StartId
                ORDER BY Id ASC
                FOR JSON PATH
            ";

            return await FetchForJsonResult(query, startId);
        }

        public async Task<string> GetUsers(int startId)
        {
            string query = $@"
                SELECT TOP 1000 [Users].[Id],
                    [Users].[AboutMe],
                    [Users].[Age],
                    [Users].[CreationDate],
                    [Users].[DisplayName],
                    [Users].[DownVotes],
                    [Users].[UpVotes],
                    [Users].[EmailHash],
                    [Users].[LastAccessDate],
                    [Users].[Location],
                    [Users].[Reputation],
                    [Users].[Views],
                    [Users].[WebsiteUrl],
                    [Users].[AccountId],
                    (
                        SELECT [Id], [Name], [Date] AS [AcuiredAt]
                        FROM [Badges]
                        WHERE [UserId] = [Users].[Id]
                        FOR JSON PATH
                    ) AS [Badges],
                    (
                        SELECT [Posts].[Id], 
                            [Posts].[Title], 
                            [Posts].[ViewCount],
                            IIF([Posts].[AcceptedAnswerId] IS NOT NULL, CAST(1 AS BIT), CAST(0 AS BIT)) AS [HasAcceptedAnswer],
                            [Posts].[CreationDate],
                            [Posts].[Score],
                            (
                                SELECT COUNT(1)
                                FROM [Posts] AS [Answers]
                                WHERE [Answers].[Id] = [Posts].[Id]
                            ) AS [AnswerCount],
                            JSON_QUERY('[' + REPLACE(REPLACE(REPLACE([Posts].[Tags],'<','""'),'>','""'),'""""','"",""') + ']') AS [Tags]
                        FROM [Posts] AS [Posts]
                        WHERE [Posts].[OwnerUserId] = [Users].[Id]
                        FOR JSON PATH
                    ) AS [Questions],
                    (
                        SELECT [Answers].[Id], 
                            [Posts].[Title], 
                            IIF([Posts].[AcceptedAnswerId] = [Answers].[Id], CAST(1 AS BIT), CAST(0 AS BIT)) AS [IsAcceptedAnswer],
                            [Answers].[CreationDate],
                            [Answers].[Score],
                            JSON_QUERY('[' + REPLACE(REPLACE(REPLACE([Posts].[Tags],'<','""'),'>','""'),'""""','"",""') + ']') AS [Tags]
                        FROM [Posts] AS [Posts]
                        INNER JOIN [Posts] AS [Answers]
                            ON [Posts].[Id] = [Answers].[ParentId]
                        WHERE [Answers].[OwnerUserId] = [Users].[Id]
                        FOR JSON PATH
                    ) AS [Answers],
                    (
                        SELECT [Votes].[Id],
                            [Votes].[PostId],
                            [Votes].[BountyAmount], 
                            [Votes].[CreationDate],
                            [VoteTypes].[Name] AS [VoteType]
                        FROM [Votes] AS [Votes] 
                        LEFT JOIN [VoteTypes] AS [VoteTypes]
                            ON [Votes].[VoteTypeId] = [VoteTypes].[Id]
                        WHERE [UserId] = [Users].[Id] 
                        FOR JSON PATH
                    ) AS [Votes],
                    (
                        SELECT [Comments].[Id],
                            [Comments].[PostId],
                            [Comments].[Text] AS [Body], 
                            [Comments].[Score],
                            [Comments].[CreationDate]
                        FROM [Comments] AS [Comments]
                        WHERE [UserId] = [Users].[Id] 
                        FOR JSON PATH
                    ) AS [Comments]
                FROM [Users] AS [Users]
                WHERE [Id] > @StartId
                ORDER BY Id ASC
                FOR JSON PATH
            ";

            return await FetchForJsonResult(query, startId);
        }

        private async Task<string> FetchForJsonResult(string query, int startId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3600;
                cmd.Parameters.Add("@StartId", SqlDbType.Int);
                cmd.Parameters["@StartId"].Value = startId;

                cmd.Connection.Open();

                var jsonResult = new StringBuilder();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        jsonResult.Append(reader.GetValue(0).ToString());
                    }
                }

                return jsonResult.ToString();
            }
        }

        public async Task<int> GetMaxPostId()
        {
            string query = "SELECT MAX(Id) FROM Posts WHERE ParentId IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<int>(query);
            }
        }

        public async Task<int> GetMaxUserId()
        {
            string query = "SELECT MAX(Id) FROM Users";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<int>(query);
            }
        }

        private void CreateIndexes()
        {
            string query = @"
                /*
                Comments:
                Changing the clustered index for Comments to PostId because Comments will mostly be viewed from the perspective of their parent post. 
                Viewing it from the user perspecive can still be done via a non clustered index on the UserId but will result in bookmark lookups.
                */
                IF EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='PK_Comments__Id' AND OBJECT_ID = OBJECT_ID('Comments')
                )
                BEGIN
                    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [PK_Comments__Id]
                END

                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_Comments_PostId' AND OBJECT_ID = OBJECT_ID('Comments')
                )
                BEGIN
                    CREATE CLUSTERED INDEX [IX_Comments_PostId] ON [dbo].[Comments] ([PostId])
                END

                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_Comments_UserId' AND OBJECT_ID = OBJECT_ID('Comments')
                )
                BEGIN
                    CREATE NONCLUSTERED INDEX [IX_Comments_UserId]
                    ON [dbo].[Comments] ([UserId])
                    INCLUDE ([Id],[PostId],[Text],[Score],[CreationDate])
                END

                /*
                Votes:
                Changing the clustered index for Votes to PostId because Votes will mostly be viewed from the perspective of their parent post. 
                Viewing it from the user perspecive can still be done via a non clustered index on the UserId but will result in bookmark lookups.
                */
                IF EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='PK_Votes__Id' AND OBJECT_ID = OBJECT_ID('Votes')
                )
                BEGIN
                    ALTER TABLE [dbo].[Votes] DROP CONSTRAINT [PK_Votes__Id]
                END

                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_Votes_PostId' AND OBJECT_ID = OBJECT_ID('Votes')
                )
                BEGIN
                    CREATE CLUSTERED INDEX [IX_Votes_PostId] ON [dbo].[Votes] ([PostId])
                END

                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_Votes_UserId' AND OBJECT_ID = OBJECT_ID('Votes')
                )
                BEGIN
                    CREATE NONCLUSTERED INDEX [IX_Votes_UserId]
                    ON [dbo].[Votes] ([UserId])
                    INCLUDE ([Id],[PostId],[BountyAmount],[VoteTypeId],[CreationDate])
                END

                /*
                Badges:
                Badges are only related to the user and will only be shown togheter with this user. And thus they will always be searched for via the UserId, that why it makes sense to change the clustered key to the UserId
                */
                IF EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='PK_Badges__Id' AND OBJECT_ID = OBJECT_ID('Badges')
                )
                BEGIN
                    ALTER TABLE [dbo].[Badges] DROP CONSTRAINT [PK_Badges__Id]
                END

                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_Badges_UserId' AND OBJECT_ID = OBJECT_ID('Badges')
                )
                BEGIN
                    CREATE CLUSTERED INDEX [IX_Badges_UserId] ON [dbo].[Badges] ([UserId])
                END

                /*
                PostLinks:
                Changing the clustered index for PostLinks to PostId because PostLinks will only be viewed from the perspective of their parent post. 
                */
                IF EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='PK_PostLinks__Id' AND OBJECT_ID = OBJECT_ID('PostLinks')
                )
                BEGIN
                    ALTER TABLE [dbo].[PostLinks] DROP CONSTRAINT [PK_PostLinks__Id]
                END

                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_PostLinks_PostId' AND OBJECT_ID = OBJECT_ID('PostLinks')
                )
                BEGIN
                    CREATE CLUSTERED INDEX [IX_PostLinks_PostId] ON [dbo].[PostLinks] ([PostId])
                END

                /*
                Posts:
                Posts will mostly be searched for via it's id but a post can also be an answer, in such a case it will be searched for by it's parent id. 
                For this case we make a non clustered index that will include all attributes that will be selected for answers so bookmark lookups will be avoided.
                Posts can also be searched for via the OwnerId of the user, thus we also create a clustered index to aid in the performance of those queries.
                */
                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_Posts_ParentId' AND OBJECT_ID = OBJECT_ID('Posts')
                )
                BEGIN
                    CREATE NONCLUSTERED INDEX [IX_Posts_ParentId]
                    ON [dbo].[Posts] ([ParentId])
                    INCLUDE ([Id],[Body],[ClosedDate],[CommunityOwnedDate],[CreationDate],[FavoriteCount],[LastActivityDate],[LastEditDate],[PostTypeId],[Score],[LastEditorUserId],[OwnerUserId])
                END

                IF NOT EXISTS(
                    SELECT 1
                    FROM sys.indexes 
                    WHERE NAME='IX_Posts_OwnerUserId' AND OBJECT_ID = OBJECT_ID('Posts')
                )
                BEGIN
                    CREATE NONCLUSTERED INDEX [IX_Posts_OwnerUserId]
                    ON [dbo].[Posts] ([OwnerUserId])
                    INCLUDE ([Id],[Title],[ViewCount],[AcceptedAnswerId],[CreationDate],[Score],[Tags])
                END
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query);
            }
        }
    }
}