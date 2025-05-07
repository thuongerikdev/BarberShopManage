import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:barbermanagemobile/data/models/blog_model.dart';

abstract class BlogRemoteDataSource {
  Future<List<BlogModel>> getAllBlogs();
  Future<BlogModel> getBlogDetail(int blogId);
}

class BlogRemoteDataSourceImpl implements BlogRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<BlogModel>> getAllBlogs() async {
    final response = await http.get(
      Uri.parse('$baseUrl/SocialBlog/getall'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonData['data'])
            .map((json) => BlogModel.fromJson(json))
            .toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách blog thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<BlogModel> getBlogDetail(int blogId) async {
    final response = await http.get(
      Uri.parse('$baseUrl/SocialBlogBussiness/detail/$blogId'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return BlogModel.fromJson(jsonData['data']);
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy chi tiết blog thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}