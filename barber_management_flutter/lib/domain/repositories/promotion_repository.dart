import 'package:barbermanagemobile/domain/entities/promotion.dart';

abstract class PromotionRepository {
  Future<List<Promotion>> getPromotions();
}