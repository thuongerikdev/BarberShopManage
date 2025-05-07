import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/data/models/user_model.dart';

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
}