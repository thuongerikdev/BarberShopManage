// lib/domain/usecases/get_commitment_image_use_case.dart
import 'package:barbermanagemobile/domain/repositories/slider_repository.dart';

class GetCommitmentImageUseCase {
  final SliderRepository repository;

  GetCommitmentImageUseCase(this.repository);

  Future<Map<String, String>> call() async {
    return await repository.getCommitmentImage();
  }
}