import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/domain/entities/social/blog.dart';
import 'package:barbermanagemobile/domain/usecases/get_blog_detail_use_case.dart';

class BlogDetailScreen extends StatefulWidget {
  final int blogId;

  const BlogDetailScreen({super.key, required this.blogId});

  @override
  _BlogDetailScreenState createState() => _BlogDetailScreenState();
}

class _BlogDetailScreenState extends State<BlogDetailScreen> {
  final GetBlogDetailUseCase _getBlogDetailUseCase = GetIt.instance<GetBlogDetailUseCase>();

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        backgroundColor: primaryColor,
        title: Text(
          'Chi Tiết Blog',
          style: TextStyle(
            color: textColor,
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w600,
          ),
        ),
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: FutureBuilder<Blog>(
        future: _getBlogDetailUseCase.call(widget.blogId),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)));
          } else if (snapshot.hasError) {
            return Center(child: Text('Lỗi: ${snapshot.error}', style: TextStyle(color: Colors.redAccent, fontSize: 14)));
          } else if (!snapshot.hasData) {
            return Center(child: Text('Không có dữ liệu blog', style: TextStyle(color: textColor, fontSize: 14)));
          }

          final blog = snapshot.data!;
          return SingleChildScrollView(
            physics: BouncingScrollPhysics(),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Blog Title
                  Text(
                    blog.blogTitle,
                    style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.w700,
                      color: textColor,
                      fontFamily: 'Poppins',
                    ),
                  ),
                  SizedBox(height: 8),
                  // Blog Description
                  Text(
                    blog.blogDescription,
                    style: TextStyle(
                      fontSize: 16,
                      fontStyle: FontStyle.italic,
                      color: Colors.grey[400],
                      fontFamily: 'Poppins',
                    ),
                  ),
                  SizedBox(height: 16),
                  // Main Blog Image
                  if (blog.blogImage.isNotEmpty)
                    ClipRRect(
                      borderRadius: BorderRadius.circular(12),
                      child: Image.network(
                        blog.blogImage,
                        width: double.infinity,
                        height: 200,
                        fit: BoxFit.cover,
                        errorBuilder: (context, error, stackTrace) => Icon(Icons.broken_image, color: Colors.redAccent, size: 50),
                      ),
                    ),
                  SizedBox(height: 16),
                  // Blog Content
                  Text(
                    blog.blogContent,
                    style: TextStyle(
                      fontSize: 16,
                      color: textColor,
                      fontFamily: 'Poppins',
                    ),
                  ),
                  SizedBox(height: 16),
                  // Blog Topics
                  if (blog.topics != null && blog.topics!.isNotEmpty) ...[
                    Text(
                      'Chủ Đề',
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.w600,
                        color: textColor,
                        fontFamily: 'Poppins',
                      ),
                    ),
                    SizedBox(height: 8),
                    ...blog.topics!
                        .map((topic) => Padding(
                              padding: const EdgeInsets.only(bottom: 12.0),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    topic.topicTitle,
                                    style: TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.w500,
                                      color: accentColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                  SizedBox(height: 4),
                                  Text(
                                    topic.topicContent,
                                    style: TextStyle(
                                      fontSize: 14,
                                      color: textColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                ],
                              ),
                            ))
                        .toList(),
                  ],
                  SizedBox(height: 16),
                  // Blog Images
                  if (blog.images != null && blog.images!.isNotEmpty) ...[
                    Text(
                      'Hình Ảnh',
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.w600,
                        color: textColor,
                        fontFamily: 'Poppins',
                      ),
                    ),
                    SizedBox(height: 8),
                    SizedBox(
                      height: 120,
                      child: ListView.builder(
                        scrollDirection: Axis.horizontal,
                        itemCount: blog.images!.length,
                        itemBuilder: (context, index) {
                          final image = blog.images![index];
                          return Padding(
                            padding: const EdgeInsets.only(right: 8.0),
                            child: ClipRRect(
                              borderRadius: BorderRadius.circular(8),
                              child: Image.network(
                                image.srcImage,
                                width: 120,
                                height: 120,
                                fit: BoxFit.cover,
                                errorBuilder: (context, error, stackTrace) => Icon(Icons.broken_image, color: Colors.redAccent),
                              ),
                            ),
                          );
                        },
                      ),
                    ),
                  ],
                  SizedBox(height: 16),
                  // Blog Contents
                  if (blog.contents != null && blog.contents!.isNotEmpty) ...[
                    Text(
                      'Nội Dung Chi Tiết',
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.w600,
                        color: textColor,
                        fontFamily: 'Poppins',
                      ),
                    ),
                    SizedBox(height: 8),
                    ...blog.contents!
                        .map((content) => Padding(
                              padding: const EdgeInsets.only(bottom: 12.0),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    content.contentTitle,
                                    style: TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.w500,
                                      color: accentColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                  SizedBox(height: 4),
                                  Text(
                                    content.content,
                                    style: TextStyle(
                                      fontSize: 14,
                                      color: textColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                ],
                              ),
                            ))
                        .toList(),
                  ],
                  SizedBox(height: 16),
                  // Blog Comments (Placeholder)
                  if (blog.comments != null && blog.comments!.isNotEmpty) ...[
                    Text(
                      'Bình Luận',
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.w600,
                        color: textColor,
                        fontFamily: 'Poppins',
                      ),
                    ),
                    SizedBox(height: 8),
                    Text(
                      'Hiện chưa có bình luận.',
                      style: TextStyle(
                        fontSize: 14,
                        color: Colors.grey[400],
                        fontFamily: 'Poppins',
                      ),
                    ),
                  ],
                  SizedBox(height: 40),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}