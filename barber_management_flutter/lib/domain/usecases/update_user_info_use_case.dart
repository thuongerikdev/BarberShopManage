import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/data/models/auth/user_model.dart';
import 'package:barbermanagemobile/domain/repositories/auth_repository.dart';

class UpdateUserInfoUseCase {
  final AuthRepository repository;

  UpdateUserInfoUseCase(this.repository);

  Future<Either<String, void>> call({
    required int userID,
    required String fullName,
    String? userName,
    String? userEmail,
    String? userPhone,
    String? dateOfBirth,
    String? gender,
  }) async {
    return await repository.updateUserInfo(
      userID: userID,
      fullName: fullName,
      userName: userName,
      userEmail: userEmail,
      userPhone: userPhone,
      dateOfBirth: dateOfBirth,
      gender: gender,
    );
  }
}