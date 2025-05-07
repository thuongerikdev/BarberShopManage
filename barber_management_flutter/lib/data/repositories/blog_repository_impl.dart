import 'package:barbermanagemobile/data/datasources/blog_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/blog.dart';
import 'package:barbermanagemobile/domain/repositories/blog_repository.dart';

class BlogRepositoryImpl implements BlogRepository {
  final BlogRemoteDataSource remoteDataSource;

  BlogRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Blog>> getAllBlogs() async {
    try {
      final blogs = await remoteDataSource.getAllBlogs();
      return blogs;
    } catch (e) {
      throw Exception('Failed to fetch blogs: $e');
    }
  }

  @override
  Future<Blog> getBlogDetail(int blogId) async {
    try {
      final blog = await remoteDataSource.getBlogDetail(blogId);
      return blog;
    } catch (e) {
      throw Exception('Failed to fetch blog detail: $e');
    }
  }
}