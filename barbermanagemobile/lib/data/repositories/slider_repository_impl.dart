// lib/data/repositories/slider_repository_impl.dart
import 'package:barbermanagemobile/data/datasources/slider_remote_data_source.dart';
import 'package:barbermanagemobile/domain/repositories/slider_repository.dart';

class SliderRepositoryImpl implements SliderRepository {
  final SliderRemoteDataSource remoteDataSource;

  SliderRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Map<String, String>>> getSliderImages() async {
    return await remoteDataSource.getSliderImages();
  }
}