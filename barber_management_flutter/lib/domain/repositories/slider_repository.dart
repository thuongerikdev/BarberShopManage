// lib/domain/repositories/slider_repository.dart
abstract class SliderRepository {
  Future<List<Map<String, String>>> getSliderImages();
  Future<Map<String, String>> getCommitmentImage(); // New method
}