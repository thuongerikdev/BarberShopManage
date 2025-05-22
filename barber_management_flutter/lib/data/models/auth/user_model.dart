import 'dart:convert';
import '../../../domain/entities/auth/user.dart';

class UserModel extends User {
  UserModel({
    required String name,
    required String userId,
    required String token,
    String? email,
    String? phoneNumber,
    String? dateOfBirth,
    String? gender,
    int? roleID,
    String? userName,
    String? avatar,
    bool? isEmp,
    bool? isEmailVerified,
    String? emailVerificationToken,
  }) : super(
          name: name,
          userId: userId,
          token: token,
          email: email,
          phoneNumber: phoneNumber,
          dateOfBirth: dateOfBirth,
          gender: gender,
          roleID: roleID,
          userName: userName,
          avatar: avatar,
          isEmp: isEmp,
          isEmailVerified: isEmailVerified,
          emailVerificationToken: emailVerificationToken,
        );

  factory UserModel.fromJson(Map<String, dynamic> json) {
    // Handle login response (data is a token string)
    if (json['data'] is String) {
      final token = json['data'] as String;
      final decoded = _decodeToken(token);
      return UserModel(
        name: decoded['name'] ?? '',
        userId: decoded['userId'] ?? '',
        token: token,
      );
    }
    // Handle getUserByID response (data is a Map)
    else if (json['data'] is Map<String, dynamic>) {
      final data = json['data'] as Map<String, dynamic>;
      final token = data['token'] as String;
      final decoded = _decodeToken(token);
      return UserModel(
        name: data['fullName'] ?? decoded['name'] ?? '',
        userId: data['userID'].toString(),
        token: token,
        email: data['email'] as String? ?? '',
        phoneNumber: data['phoneNumber'] as String? ?? '',
        dateOfBirth: data['dateOfBirth'] as String? ?? '',
        gender: data['gender'] as String? ?? '',
        roleID: data['roleID'] as int?,
        userName: data['userName'] as String? ?? '',
        avatar: data['avatar'] as String? ?? '',
        isEmp: data['isEmp'] as bool?,
        isEmailVerified: data['isEmailVerified'] as bool?,
        emailVerificationToken: data['emailVerificationToken'] as String? ?? '',
      );
    }
    // Handle registration response (data is an empty array)
    else if (json['data'] is List && (json['data'] as List).isEmpty) {
      return UserModel(
        name: '',
        userId: '0',
        token: '',
        email: '',
        phoneNumber: '',
        dateOfBirth: '',
        gender: '',
      );
    } else {
      throw Exception('Invalid response format: data field is neither a String, Map, nor empty List');
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
      'roleID': roleID,
      'userName': userName,
      'avatar': avatar,
      'isEmp': isEmp,
      'isEmailVerified': isEmailVerified,
      'emailVerificationToken': emailVerificationToken,
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