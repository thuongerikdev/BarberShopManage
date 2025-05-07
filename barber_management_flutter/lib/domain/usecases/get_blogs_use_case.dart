import 'package:barbermanagemobile/domain/entities/blog.dart';
import 'package:barbermanagemobile/domain/repositories/blog_repository.dart';

class GetBlogsUseCase {
  final BlogRepository repository;

  GetBlogsUseCase(this.repository);

  Future<List<Blog>> call() async {
    return await repository.getAllBlogs();
  }
}