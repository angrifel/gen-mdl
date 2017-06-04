# gen-mdl 

[![Build status](https://ci.appveyor.com/api/projects/status/7m9l9ralcadqwhua/branch/master?svg=true)](https://ci.appveyor.com/project/angrifel/gen-mdl/branch/master)

## What is it?

**gen-mdl** is a code generator that takes a data model definition as input and produces code in C# and TypeScript for that model.

## What is it good for?

It's great at centralizing knowledge to keep data model classes in client side (browser) and server-side updated.

## Our philosophy
We attempt to follow [DRY](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself) and [KISS](https://en.wikipedia.org/wiki/KISS_principle) principles.

Our core principles
 - Knowledge should be centered in a single place. 
   - *You shouldn't have to  look at files in different languages to understand your model.*
 - Data modeling should be simple.
   - *You should only provide as much detail as is actually needed, but rely on the defaults to do the rest.*


 
## How does gen-mdl work?

 - You write model definitions in YAML.
 - You invoke gen-mdl with your model definition.
 - You get code. :)

## What does a model definition look like?

A model definition for a blog application looks like this.

```yaml
%YAML 1.1
---
targets : 
    csharp : 
        path         : Blog.Model\\Data
        namespace    : Blog.Model.Data
        type_aliases : 
            id_t : int 

    typescript : 
        path         : client\\app\\model
        type_aliases : 
            id_t : int

enums : 
    blog_post_status : [draft, final]

entities :
    author :
        members: 
            id    : id_t
            name  : string
            alias : string

    blog :
        members:
            id     : id_t
            title  : string
            posts  : 
                type          : blog_post
                is_collection : true
            author : author

    blog_post : 
        members:
            id             : id_t
            date_published : { type: datetime, is_nullable: true }
            description    : { type: string, is_nullable: true }
            comments       :
                type          : comment
                is_collection : true
            status         : blog_post_status

    comment :
        members:
            id   : id_t
            text : string
            shared_in_fb : { type: bool, exclude: [typescript] }

```
## What do I get after invoking gen-mdl?

For the model above you would get the following files

- Blog.Model\Data\Author.cs
```csharp
namespace Blog.Model.Data
{
  using System.ComponentModel.DataAnnotations;

  public class Author
  {
    public int Id { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string Alias { get; set; }
  }
}
```

- Blog.Model\Data\Blog.cs
```csharp
namespace Blog.Model.Data
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  public class Blog
  {
    public int Id { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string Title { get; set; }

    [Required]
    public IList<BlogPost> Posts { get; set; }

    [Required]
    public Author Author { get; set; }
  }
}
```

- Blog.Model\Data\BlogPost.cs
```csharp
namespace Blog.Model.Data
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  public class BlogPost
  {
    public int Id { get; set; }

    public DateTimeOffset? DatePublished { get; set; }

    public string Description { get; set; }

    [Required]
    public IList<Comment> Comments { get; set; }

    public BlogPostStatus Status { get; set; }
  }
}
```

- Blog.Model\Data\BlogPostStatus.cs
```csharp
namespace Blog.Model.Data
{
  public enum BlogPostStatus
  {
    Draft,
    Final
  }
}
```

- Blog.Model\Data\Comment.cs
```csharp
namespace Blog.Model.Data
{
  using System.ComponentModel.DataAnnotations;

  public class Comment
  {
    public int Id { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string Text { get; set; }

    public bool SharedInFb { get; set; }
  }
}
```

- client\app\model\author.ts
```typescript
export default class Author {
  id : number;
  name : string;
  alias : string;
}
```

- client\app\model\blog-post-status.ts
```typescript
enum BlogPostStatus {
  Draft,
  Final
}

export default BlogPostStatus;
```

- client\app\model\blog-post.ts
```typescript
import BlogPostStatus from './blog-post-status'
import Comment from './comment'

export default class BlogPost {
  id : number;
  datePublished : Date;
  description : string;
  comments : Comment[];
  status : BlogPostStatus;
}
```

- client\app\model\blog.ts
```typescript
import BlogPost from './blog-post'
import Author from './author'

export default class Blog {
  id : number;
  title : string;
  posts : BlogPost[];
  author : Author;
}
```

- client\app\model\comment.ts
```typescript
export default class Comment {
  id : number;
  text : string;
}
```

- client\app\model\index.ts
```typescript
export * from './blog-post-status';
export * from './author';
export * from './blog';
export * from './blog-post';
export * from './comment';
```

## Requirements

* [.NET Framework](https://www.microsoft.com/en-us/download/details.aspx?id=17718) 4 or higher.

