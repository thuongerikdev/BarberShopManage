import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/data/models/auth/user_model.dart';
import 'dart:io';

abstract class AuthRepository {
  Future<Either<String, UserModel>> login(String email, String password);
  Future<Either<String, UserModel>> getUserByID(int userID);
  Future<Either<String, UserModel>> register({
    required int roleID,
    required String userName,
    required String password,
    required String email,
    required String phoneNumber,
    required String fullName,
    required String dateOfBirth,
    required String gender,
  });
  Future<Either<String, void>> updateAvatar(int userID, File image);
  Future<Either<String, void>> updateUserInfo({
    required int userID,
    required String fullName,
    String? userName,
    String? userEmail,
    String? userPhone,
    String? dateOfBirth,
    String? gender,
  });
}

