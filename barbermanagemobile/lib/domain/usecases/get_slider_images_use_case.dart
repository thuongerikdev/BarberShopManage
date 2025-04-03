// lib/domain/usecases/get_slider_images_use_case.dart
import 'package:barbermanagemobile/domain/repositories/slider_repository.dart';

class GetSliderImagesUseCase {
  final SliderRepository repository;

  GetSliderImagesUseCase(this.repository);

  Future<List<Map<String, String>>> call() async {
    return await repository.getSliderImages();
  }
}