import 'package:barbermanagemobile/domain/entities/blog.dart';
import 'package:barbermanagemobile/domain/repositories/blog_repository.dart';

class GetBlogDetailUseCase {
  final BlogRepository repository;

  GetBlogDetailUseCase(this.repository);

  Future<Blog> call(int blogId) async {
    return await repository.getBlogDetail(blogId);
  }
}