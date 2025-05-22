class BlogImage {
  final String srcImage;
  final int position;

  BlogImage({
    required this.srcImage,
    required this.position,
  });

  factory BlogImage.fromJson(Map<String, dynamic> json) {
    return BlogImage(
      srcImage: json['srcImage'] as String,
      position: json['position'] as int,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'srcImage': srcImage,
      'position': position,
    };
  }
}

class BlogContent {
  final String contentTitle;
  final String content;
  final int position;

  BlogContent({
    required this.contentTitle,
    required this.content,
    required this.position,
  });

  factory BlogContent.fromJson(Map<String, dynamic> json) {
    return BlogContent(
      contentTitle: json['contentTitle'] as String,
      content: json['content'] as String,
      position: json['position'] as int,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'contentTitle': contentTitle,
      'content': content,
      'position': position,
    };
  }
}

class BlogTopic {
  final String topicTitle;
  final String topicContent;
  final int position;

  BlogTopic({
    required this.topicTitle,
    required this.topicContent,
    required this.position,
  });

  factory BlogTopic.fromJson(Map<String, dynamic> json) {
    return BlogTopic(
      topicTitle: json['topicTitle'] as String,
      topicContent: json['topicContent'] as String,
      position: json['position'] as int,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'topicTitle': topicTitle,
      'topicContent': topicContent,
      'position': position,
    };
  }
}

class BlogComment {
  // Placeholder for comments - currently empty in the API response
  BlogComment();

  factory BlogComment.fromJson(Map<String, dynamic> json) {
    return BlogComment();
  }

  Map<String, dynamic> toJson() {
    return {};
  }
}

class Blog {
  final int blogID;
  final String blogTitle;
  final String blogContent;
  final String blogStatus;
  final String blogDescription;
  final String blogImage;
  final int blogLike;
  final String blogDate;
  final String updateAt;
  final List<BlogImage>? images;
  final List<BlogContent>? contents;
  final List<BlogTopic>? topics;
  final List<BlogComment>? comments;

  Blog({
    required this.blogID,
    required this.blogTitle,
    required this.blogContent,
    required this.blogStatus,
    required this.blogDescription,
    required this.blogImage,
    required this.blogLike,
    required this.blogDate,
    required this.updateAt,
    this.images,
    this.contents,
    this.topics,
    this.comments,
  });

  factory Blog.fromJson(Map<String, dynamic> json) {
    return Blog(
      blogID: json['blogID'] as int,
      blogTitle: json['blogTitle'] as String,
      blogContent: json['blogContent'] as String,
      blogStatus: json['blogStatus'] as String,
      blogDescription: json['blogDescription'] as String,
      blogImage: json['blogImage'] as String,
      blogLike: json['blogLike'] as int,
      blogDate: json['blogDate'] as String,
      updateAt: json['updateAt'] as String,
      images: json['Images'] != null
          ? List<Map<String, dynamic>>.from(json['Images'])
              .map((item) => BlogImage.fromJson(item))
              .toList()
          : null,
      contents: json['Contents'] != null
          ? List<Map<String, dynamic>>.from(json['Contents'])
              .map((item) => BlogContent.fromJson(item))
              .toList()
          : null,
      topics: json['Topics'] != null
          ? List<Map<String, dynamic>>.from(json['Topics'])
              .map((item) => BlogTopic.fromJson(item))
              .toList()
          : null,
      comments: json['Comments'] != null
          ? List<Map<String, dynamic>>.from(json['Comments'])
              .map((item) => BlogComment.fromJson(item))
              .toList()
          : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'blogID': blogID,
      'blogTitle': blogTitle,
      'blogContent': blogContent,
      'blogStatus': blogStatus,
      'blogDescription': blogDescription,
      'blogImage': blogImage,
      'blogLike': blogLike,
      'blogDate': blogDate,
      'updateAt': updateAt,
      'Images': images?.map((item) => item.toJson()).toList(),
      'Contents': contents?.map((item) => item.toJson()).toList(),
      'Topics': topics?.map((item) => item.toJson()).toList(),
      'Comments': comments?.map((item) => item.toJson()).toList(),
    };
  }
}