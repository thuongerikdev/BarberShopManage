// lib/data/datasources/employee_remote_data_source.dart
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';

class EmployeeRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL'); // Lấy từ .env

  Future<List<Map<String, String>>> getEmployees() async {
    final url = Uri.parse('$baseUrl/AuthEmp/getAllUserEmp');
    final response = await http.get(url, headers: {'accept': '*/*'});

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 0) {
        final List<dynamic> data = jsonData['data'];
        return data.map((item) => {
          'name': item['fullName'] as String,
          'image': item['avatar'] as String,
        }).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách nhân viên thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}