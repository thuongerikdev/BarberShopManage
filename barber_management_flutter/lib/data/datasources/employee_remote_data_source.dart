import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';

abstract class EmployeeRemoteDataSource {
  Future<List<Map<String, String>>> getEmployees();
  Future<Map<String, dynamic>> getEmpByUserID(int userID);
  Future<Map<String, dynamic>> getEmployeeById(int empID);
}

class EmployeeRemoteDataSourceImpl implements EmployeeRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<Map<String, String>>> getEmployees() async {
    final url = Uri.parse('$baseUrl/AuthEmp/getAllUserEmp');
    final response = await http.get(url, headers: {'accept': '*/*'});

    print('getAllUserEmp API response: ${response.statusCode} ${response.body}');

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final List<dynamic> data = jsonData['data'];
        final List<Map<String, String>> employees = [];

        for (var item in data) {
          final userID = item['userID']?.toString();
          // final empID = item['empID']?.toString();
          final name = item['fullName']?.toString() ?? 'Unknown';
          final image = item['avatar']?.toString() ?? 'https://via.placeholder.com/80';

          if (userID == null || int.tryParse(userID) == null ) {
            print('Invalid userID: $userID or empID:  in item: $item');
            continue;
          }

          employees.add({
            'userID': userID,
            'name': name,
            'image': image,
          });
        }

        return employees;
      } else {
        throw Exception(jsonData['errorMessage'] ?? 'Lấy danh sách nhân viên thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<Map<String, dynamic>> getEmpByUserID(int userID) async {
    final url = Uri.parse('$baseUrl/AuthEmp/getEmpByUserID/$userID');
    final response = await http.get(url, headers: {'accept': '*/*'});

    print('getEmpByUserID/$userID API response: ${response.statusCode} ${response.body}');

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final empData = jsonData['data'] as Map<String, dynamic>;
        return {
          'empID': empData['empID']?.toString() ?? 'N/A',
          'userID': empData['userID']?.toString() ?? userID.toString(),
          'fullName': empData['fullName'] ?? 'Unknown',
          'image': empData['image'] ?? 'https://via.placeholder.com/120',
          'email': empData['email'] ?? 'Không có email',
          'phone': empData['phone'] ?? 'Không có số điện thoại',
          'positionName': empData['positionName'] ?? 'Nhân viên',
          'specialtyName': empData['specialtyName'] ?? 'Không có chuyên môn',
          'branchName': empData['branchName'] ?? 'Không có chi nhánh',
          'salary': empData['salary']?.toString() ?? '0',
          'bonusSalary': empData['bonusSalary']?.toString() ?? '0',
          'startDate': empData['startDate'] ?? '',
          'gender': empData['gender'] ?? 'Không xác định',
          'dateOfBirth': empData['dateOfBirth'] ?? '',
          'status': empData['status'] ?? 'Không xác định',
          'branchesImage': empData['branchesImage'] ?? 'https://via.placeholder.com/120',
        };
      } else {
        throw Exception(jsonData['errorMessage'] ?? 'Lấy thông tin nhân viên thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<Map<String, dynamic>> getEmployeeById(int empID) async {
    final url = Uri.parse('$baseUrl/AuthEmp/getEmployeeById/$empID');
    final response = await http.get(url, headers: {'accept': '*/*'});

    // print('getEmployeeById/$empID API response: ${response.statusCode} ${response.body}');

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final data = jsonData['data'] as Map<String, dynamic>;
        return {
          'empID': data['empID']?.toString() ?? empID.toString(),
          'userID': data['userID']?.toString(),
          'fullName': data['fullName'] ?? 'Unknown',
          'image': data['image'] ?? 'https://via.placeholder.com/120',
          'positionName': data['positionName'] ?? 'Nhân viên',
          'email': data['email'] ?? 'Không có email',
          'phone': data['phone'] ?? 'Không có số điện thoại',
          'branchName': data['branchName'] ?? 'Không có chi nhánh',
          'specialtyName': data['specialtyName'] ?? 'Không có chuyên môn',
          'salary': data['salary']?.toString() ?? '0',
          'startDate': data['startDate'] ?? '',
          'gender': data['gender'] ?? 'Không xác định',
          'dateOfBirth': data['dateOfBirth'] ?? '',
        };
      } else {
        throw Exception(jsonData['errorMessage'] ?? 'Lấy thông tin nhân viên thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}