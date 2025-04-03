// lib/data/datasources/auth_remote_data_source.dart
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../models/user_model.dart';

abstract class AuthRemoteDataSource {
  Future<UserModel> login(String email, String password);
}

class AuthRemoteDataSourceImpl implements AuthRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL'); // Lấy từ .env

  @override
  Future<UserModel> login(String email, String password) async {
    final url = Uri.parse('$baseUrl/AuthUser/login');
    final headers = {'Content-Type': 'application/json'};
    final body = jsonEncode({
      'userName': email, // Giả sử backend dùng 'userName' thay vì 'email'
      'password': password,
    });

    final response = await http.post(
      url,
      headers: headers,
      body: body,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 0) {
        return UserModel.fromJson(jsonData);
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Đăng nhập thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}