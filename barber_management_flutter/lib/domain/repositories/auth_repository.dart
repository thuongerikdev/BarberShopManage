import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/data/models/user_model.dart';

abstract class AuthRepository {
  Future<Either<String, UserModel>> login(String email, String password);
  Future<Either<String, UserModel>> getUserByID(int userID);
}