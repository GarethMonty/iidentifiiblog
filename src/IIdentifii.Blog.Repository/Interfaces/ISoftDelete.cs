﻿namespace IIdentifii.Blog.Repository
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }

        DateTimeOffset DeletedAt { get; set; }
    }
}
