import 'package:barbermanagemobile/data/datasources/promotion_remote_data_source.dart';
import 'package:barbermanagemobile/data/models/promotion_model.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';

class PromotionRepositoryImpl implements PromotionRepository {
  final PromotionRemoteDataSource remoteDataSource;

  PromotionRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<PromotionModel>> getPromotions() async {
    try {
      final promotions = await remoteDataSource.getPromotions();
      return promotions;
    } catch (e) {
      throw Exception('Failed to fetch promotions: $e');
    }
  }
}