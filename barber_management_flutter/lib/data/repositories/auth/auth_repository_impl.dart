import 'package:dartz/dartz.dart';
import '../../../domain/repositories/auth_repository.dart';
import '../../datasources/auth/auth_remote_data_source.dart';
import '../../models/auth/user_model.dart';
import 'dart:io';


class AuthRepositoryImpl implements AuthRepository {
  final AuthRemoteDataSource remoteDataSource;

  AuthRepositoryImpl(this.remoteDataSource);

  @override
  Future<Either<String, UserModel>> login(String email, String password) async {
    try {
      final userModel = await remoteDataSource.login(email, password);
      return Right(userModel);
    } catch (e) {
      return Left(e.toString());
    }
  }

  @override
  Future<Either<String, UserModel>> getUserByID(int userID) async {
    try {
      final userModel = await remoteDataSource.getUserByID(userID);
      return Right(userModel);
    } catch (e) {
      return Left(e.toString());
    }
  }

  @override
  Future<Either<String, UserModel>> register({
    required int roleID,
    required String userName,
    required String password,
    required String email,
    required String phoneNumber,
    required String fullName,
    required String dateOfBirth,
    required String gender,
  }) async {
    try {
      final userModel = await remoteDataSource.register(
        roleID: roleID,
        userName: userName,
        password: password,
        email: email,
        phoneNumber: phoneNumber,
        fullName: fullName,
        dateOfBirth: dateOfBirth,
        gender: gender,
      );
      return Right(userModel);
    } catch (e) {
      return Left(e.toString());
    }
  }

  @override
  Future<Either<String, void>> updateAvatar(int userID, File image) async {
    try {
      await remoteDataSource.updateAvatar(userID, image);
      return Right(null);
    } catch (e) {
      return Left(e.toString());
    }
  }

  @override
  Future<Either<String, void>> updateUserInfo({
    required int userID,
    required String fullName,
    String? userName,
    String? userEmail,
    String? userPhone,
    String? dateOfBirth,
    String? gender,
  }) async {
    try {
      final result = await remoteDataSource.updateUserInfo(
        userID: userID,
        fullName: fullName,
        userName: userName,
        userEmail: userEmail,
        userPhone: userPhone,
        dateOfBirth: dateOfBirth,
        gender: gender,
      );
      return Right(result);
    } catch (e) {
      return Left(e.toString());
    }
  }
}