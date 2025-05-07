import 'package:barbermanagemobile/domain/entities/blog.dart';

class BlogModel extends Blog {
  BlogModel({
    required int blogID,
    required String blogTitle,
    required String blogContent,
    required String blogStatus,
    required String blogDescription,
    required String blogImage,
    required int blogLike,
    required String blogDate,
    required String updateAt,
    List<BlogImage>? images,
    List<BlogContent>? contents,
    List<BlogTopic>? topics,
    List<BlogComment>? comments,
  }) : super(
          blogID: blogID,
          blogTitle: blogTitle,
          blogContent: blogContent,
          blogStatus: blogStatus,
          blogDescription: blogDescription,
          blogImage: blogImage,
          blogLike: blogLike,
          blogDate: blogDate,
          updateAt: updateAt,
          images: images,
          contents: contents,
          topics: topics,
          comments: comments,
        );

  factory BlogModel.fromJson(Map<String, dynamic> json) {
    return BlogModel(
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

  @override
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