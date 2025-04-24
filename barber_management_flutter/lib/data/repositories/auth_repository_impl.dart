import 'package:dartz/dartz.dart';
import '../../domain/repositories/auth_repository.dart';
import '../datasources/auth_remote_data_source.dart';
import '../models/user_model.dart';

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
}