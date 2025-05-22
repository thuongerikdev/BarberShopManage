import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../../models/auth/user_model.dart';
import 'dart:io';

abstract class AuthRemoteDataSource {
  Future<UserModel> login(String email, String password);
  Future<UserModel> getUserByID(int userID);
  Future<UserModel> register({
    required int roleID,
    required String userName,
    required String password,
    required String email,
    required String phoneNumber,
    required String fullName,
    required String dateOfBirth,
    required String gender,
  });
  Future<void> updateAvatar(int userID, File image);
  Future<void> updateUserInfo({
    required int userID,
    required String fullName,
    String? userName,
    String? userEmail,
    String? userPhone,
    String? dateOfBirth,
    String? gender,
  });
}

class AuthRemoteDataSourceImpl implements AuthRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<UserModel> login(String email, String password) async {
    final url = Uri.parse('$baseUrl/AuthUser/login');
    final headers = {'Content-Type': 'application/json'};
    final body = jsonEncode({
      'userName': email,
      'password': password,
    });

    final response = await http.post(
      url,
      headers: headers,
      body: body,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return UserModel.fromJson(jsonData);
      } else {
        throw jsonData['errorMessager'] ?? 'Đăng nhập thất bại';
      }
    } else {
      final jsonData = jsonDecode(response.body);
      throw jsonData['errorMessager'] ?? 'Lỗi server: ${response.statusCode}';
    }
  }

  @override
  Future<UserModel> getUserByID(int userID) async {
    final url = Uri.parse('$baseUrl/AuthUser/get/$userID');
    final headers = {'Content-Type': 'application/json'};

    final response = await http.get(
      url,
      headers: headers,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return UserModel.fromJson(jsonData);
      } else {
        throw jsonData['errorMessager'] ?? 'Lấy thông tin người dùng thất bại';
      }
    } else {
      final jsonData = jsonDecode(response.body);
      throw jsonData['errorMessager'] ?? 'Lỗi server: ${response.statusCode}';
    }
  }

  @override
  Future<UserModel> register({
    required int roleID,
    required String userName,
    required String password,
    required String email,
    required String phoneNumber,
    required String fullName,
    required String dateOfBirth,
    required String gender,
  }) async {
    final url = Uri.parse('$baseUrl/AuthUser/register');
    final headers = {
      'accept': '*/*',
      'Content-Type': 'application/json',
    };
    final body = jsonEncode({
      'roleID': roleID,
      'userName': userName,
      'password': password,
      'email': email,
      'phoneNumber': phoneNumber,
      'fullName': fullName,
      'dateOfBirth': dateOfBirth,
      'gender': gender,
    });

    final response = await http.post(
      url,
      headers: headers,
      body: body,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return UserModel(
          name: fullName,
          userId: '0',
          token: '',
          email: email,
          phoneNumber: phoneNumber,
          dateOfBirth: dateOfBirth,
          gender: gender,
        );
      } else {
        throw jsonData['errorMessager'] ?? 'Đăng ký thất bại';
      }
    } else {
      final jsonData = jsonDecode(response.body);
      throw jsonData['errorMessager'] ?? 'Lỗi server: ${response.statusCode}';
    }
  }

  @override
  Future<void> updateAvatar(int userID, File image) async {
    final url = Uri.parse('$baseUrl/AuthUser/updateAvatar');
    var request = http.MultipartRequest('PUT', url);

    request.fields['userID'] = userID.toString();
    request.files.add(await http.MultipartFile.fromPath('file', image.path));

    request.headers['accept'] = '*/*';

    final response = await request.send();
    final responseBody = await response.stream.bytesToString();
    final jsonData = jsonDecode(responseBody);

    if (jsonData['errorCode'] == 200) {
      return;
    } else {
      throw jsonData['errorMessager'] ?? 'Cập nhật ảnh đại diện thất bại';
    }
  }

  @override
  Future<void> updateUserInfo({
    required int userID,
    required String fullName,
    String? userName,
    String? userEmail,
    String? userPhone,
    String? dateOfBirth,
    String? gender,
  }) async {
    final url = Uri.parse('$baseUrl/AuthUser/updateBasicUserInfo');
    final headers = {
      'accept': '*/*',
      'Content-Type': 'application/json',
    };
    final body = jsonEncode({
      'userID': userID,
      'userName': userName,
      'userEmail': userEmail,
      'userPhone': userPhone,
      'dateOfBirth': dateOfBirth,
      'gender': gender,
      'fullName': fullName,
    });

    final response = await http.put(
      url,
      headers: headers,
      body: body,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return;
      } else {
        throw jsonData['errorMessager'] ?? 'Cập nhật thông tin thất bại';
      }
    } else {
      final jsonData = jsonDecode(response.body);
      throw jsonData['errorMessager'] ?? 'Lỗi server: ${response.statusCode}';
    }
  }
}