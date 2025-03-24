import 'dart:convert';
import '../../domain/entities/user.dart';

class UserModel extends User {
  UserModel({
    required String name,
    required String userId,
    required String token,
  }) : super(name: name, userId: userId, token: token);

  factory UserModel.fromJson(Map<String, dynamic> json) {
    final token = json['data'] as String;
    final decoded = _decodeToken(token);
    return UserModel(
      name: decoded['name'] ?? '',
      userId: decoded['userId'] ?? '',
      token: token,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'name': name,
      'userId': userId,
      'token': token,
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