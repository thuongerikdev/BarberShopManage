import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';

abstract class EmployeeRemoteDataSource {
  Future<List<Map<String, String>>> getEmployees();
}

class EmployeeRemoteDataSourceImpl implements EmployeeRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<Map<String, String>>> getEmployees() async {
    final url = Uri.parse('$baseUrl/AuthEmp/getAllUserEmp');
    final response = await http.get(url, headers: {'accept': '*/*'});

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final List<dynamic> data = jsonData['data'];
        return data.map((item) {
          final name = item['fullName']?.toString() ?? 'Unknown';
          final image = item['avatar']?.toString() ?? '';
          return {
            'name': name,
            'image': image,
          };
        }).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách nhân viên thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}