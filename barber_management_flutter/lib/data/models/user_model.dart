import 'dart:convert';
import '../../domain/entities/user.dart';

class UserModel extends User {
  final String? email;
  final String? phoneNumber;
  final String? dateOfBirth;
  final String? gender;

  UserModel({
    required String name,
    required String userId,
    required String token,
    this.email,
    this.phoneNumber,
    this.dateOfBirth,
    this.gender,
  }) : super(
            name: name,
            userId: userId,
            token: token,
            email: email,
            phoneNumber: phoneNumber,
            dateOfBirth: dateOfBirth,
            gender: gender);

  factory UserModel.fromJson(Map<String, dynamic> json) {
    // Check if 'data' is a String (likely the token from login response)
    if (json['data'] is String) {
      final token = json['data'] as String;
      final decoded = _decodeToken(token);
      return UserModel(
        name: decoded['name'] ?? '',
        userId: decoded['userId'] ?? '',
        token: token,
      );
    }
    // Otherwise, 'data' is a Map (from getUserByID response)
    else if (json['data'] is Map<String, dynamic>) {
      final data = json['data'] as Map<String, dynamic>;
      final token = data['token'] as String;
      final decoded = _decodeToken(token);
      return UserModel(
        name: data['fullName'] ?? decoded['name'] ?? '',
        userId: data['userID'].toString() ,
        token: token,
        email: data['email'] as String? ?? '',
        phoneNumber: data['phoneNumber'] as String? ?? '',
        dateOfBirth: data['dateOfBirth'] as String? ?? '',
        gender: data['gender'] as String? ?? '',
      );
    } else {
      throw Exception(
          'Invalid response format: data field is neither a String nor a Map');
    }
  }

  Map<String, dynamic> toJson() {
    return {
      'name': name,
      'userId': userId,
      'token': token,
      'email': email,
      'phoneNumber': phoneNumber,
      'dateOfBirth': dateOfBirth,
      'gender': gender,
    };
  }

  static Map<String, dynamic> _decodeToken(String token) {
    final parts = token.split('.');
    if (parts.length != 3) {
      throw Exception('Invalid token');
    }
    final payload = parts[1];
    final decoded = utf8.decode(base64Url.decode(base64Url.normalize(payload)));
    return jsonDecode(decoded);
  }
}
