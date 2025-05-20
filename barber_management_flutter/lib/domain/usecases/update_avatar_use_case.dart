import 'package:dartz/dartz.dart';
import '../../domain/repositories/auth_repository.dart';
import 'dart:io';

class UpdateAvatarUseCase {
  final AuthRepository repository;

  UpdateAvatarUseCase(this.repository);

  Future<Either<String, void>> call(int userID, File image) async {
    return await repository.updateAvatar(userID, image);
  }
}