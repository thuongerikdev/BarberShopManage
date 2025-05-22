import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/data/models/auth/user_model.dart';
import 'package:barbermanagemobile/domain/repositories/auth_repository.dart';

class RegisterUseCase {
  final AuthRepository repository;

  RegisterUseCase(this.repository);

  Future<Either<String, UserModel>> call({
    required int roleID,
    required String userName,
    required String password,
    required String email,
    required String phoneNumber,
    required String fullName,
    required String dateOfBirth,
    required String gender,
  }) async {
    return await repository.register(
      roleID: roleID,
      userName: userName,
      password: password,
      email: email,
      phoneNumber: phoneNumber,
      fullName: fullName,
      dateOfBirth: dateOfBirth,
      gender: gender,
    );
  }
}