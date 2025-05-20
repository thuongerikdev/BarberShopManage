import 'package:barbermanagemobile/data/datasources/promotion_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/promotion.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';

class PromotionRepositoryImpl implements PromotionRepository {
  final PromotionRemoteDataSource remoteDataSource;

  PromotionRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Promotion>> getPromotions() async {
    try {
      final promotions = await remoteDataSource.getPromotions();
      return promotions;
    } catch (e) {
      throw Exception('Failed to fetch promotions: $e');
    }
  }
}