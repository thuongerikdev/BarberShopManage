import 'package:barbermanagemobile/domain/entities/blog.dart';

abstract class BlogRepository {
  Future<List<Blog>> getAllBlogs();
  Future<Blog> getBlogDetail(int blogId);
}